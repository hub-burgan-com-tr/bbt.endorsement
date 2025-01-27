using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Entities;
using MediatR;
using Serilog;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;

namespace Worker.App.Application.Workers.Commands.UploadContractDocumentInstances
{
    public class UploadContractDocumentInstanceCommand : IRequest<Response<UploadContractDocumentInstanceResponse>>
    {
        public string OrderId { get; set; }
        public string AuthToken { get; set; }
        public string ToBusinessLine { get; set; }
        public string ToUserReference { get; set; }
        public string ToCustomerNo { get; set; }
        public string FormId { get; set; }
    }

    public class UploadContractDocumentInstanceCommandHandler : IRequestHandler<UploadContractDocumentInstanceCommand, Response<UploadContractDocumentInstanceResponse>>
    {
        private readonly IApplicationDbContext _context;

        public UploadContractDocumentInstanceCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<UploadContractDocumentInstanceResponse>> Handle(UploadContractDocumentInstanceCommand request, CancellationToken cancellationToken)
        {
            var orderGroup = _context.OrderGroups.Where(x => x.OrderMaps.Select(z => z.OrderId).Contains(request.OrderId)).FirstOrDefault();
            Guid contractInstanceId = Guid.NewGuid();

            if (orderGroup != null && orderGroup.OrderMaps.Count > 1)
            {
                var dependentOrderId = Guid.Parse(orderGroup.OrderMaps.OrderBy(x => x.Created).Select(x => x.OrderId).FirstOrDefault());
                contractInstanceId = _context.ContractStarts.Where(x => x.OrderId == dependentOrderId).Select(x => x.ContractInstanceId).FirstOrDefault();
            }

            Log.ForContext("ContractInstanceId", contractInstanceId).Information($"UploadContractDocumentInstanceCommand Started.");
            var orderDocuments = _context.Documents.Where(x => x.OrderId == request.OrderId).ToList();
            var documentNames = orderDocuments.Select(x => x.Name.Replace(".pdf", "")).ToList();

            var maps = _context.ContractMaps.Where(x => documentNames.Contains(x.EndorsementCode) && x.RequiresFullMatch)
            .GroupBy(x => x.ContractCode)
            .Select(g => new
            {
                ContractCode = g.Key,
                Items = g.ToList()
            }).ToDictionary(x => x.ContractCode, x => x.Items); //ASK: serbest döküman için ContractCode boş bırakma?

            var currentContract = new KeyValuePair<string, List<ContractMap>>();

            foreach (var contract in maps)
            {
                var fullMatchList = contract.Value.Select(x => x.EndorsementCode).ToList();
                Log.Information("FullMatchList: " + String.Join(';', fullMatchList));
                Log.Information("DocumentNames: " + String.Join(';', documentNames));
                bool areEqual = new HashSet<string>(fullMatchList).SetEquals(documentNames);
                if (areEqual)
                {
                    currentContract = contract;
                    Log.ForContext("ContractInstanceId", contractInstanceId).ForContext("ContractCode", currentContract.Key).Information($"UploadContractDocumentInstanceCommand Contract Found From RequiresFullMatch.");
                    break;
                }
            }

            if (String.IsNullOrEmpty(currentContract.Key))
            {
                currentContract = _context.ContractMaps.Where(x => documentNames.Contains(x.EndorsementCode) && !x.RequiresFullMatch)
                .GroupBy(x => x.ContractCode).Select(g => new
                {
                    ContractCode = g.Key,
                    Items = g.ToList()
                }).ToDictionary(x => x.ContractCode, x => x.Items).FirstOrDefault();
                Log.ForContext("ContractInstanceId", contractInstanceId).ForContext("ContractCode", currentContract.Key).Information($"UploadContractDocumentInstanceCommand Contract Found From Not RequiresFullMatch.");
            }

            if (String.IsNullOrEmpty(currentContract.Key))
            {
                throw new Exception("ContractCode not found!");
            }

            var config = _context.Configs.FirstOrDefault(x => x.OrderId == request.OrderId);
            config.ContractParameters = contractInstanceId + ";" + currentContract.Key + ";tr-TR";
            _context.Configs.Update(config);

            var contractDocumentCodes = currentContract.Value.Select(x => x.DocumentCode).ToList();
            _context.ContractStarts.Add(new ContractStart
            {
                ContractStartId = Guid.NewGuid(),
                ContractInstanceId = contractInstanceId,
                OrderId = Guid.Parse(request.OrderId),
                ContractDocuments = String.Join(';', contractDocumentCodes)
            });
            _context.SaveChanges();

            var client = new HttpClient();
            var formSource = "";
            if (!String.IsNullOrEmpty(request.FormId))
            {
                formSource = _context.FormDefinitions.Where(x => x.FormDefinitionId == request.FormId).Select(x => x.Source).FirstOrDefault();
            }

            if (String.IsNullOrEmpty(formSource) || formSource == "file")
            {
                foreach (var orderDoc in orderDocuments)
                {
                    if (orderDoc.Type != "PlainText")
                    {
                        var currentMap = currentContract.Value.FirstOrDefault(x => x.EndorsementCode == orderDoc.Name.Replace(".pdf", ""));

                        UploadContractDocumentInstanceModel uploadDoc = new UploadContractDocumentInstanceModel
                        {
                            ContractCode = currentContract.Key,
                            ContractInstanceId = contractInstanceId,
                        };

                        uploadDoc.DocumentInstanceId = Guid.NewGuid();
                        uploadDoc.DocumentCode = currentMap.DocumentCode;
                        uploadDoc.DocumentVersion = currentMap.DocumentVersion;
                        uploadDoc.Notes = new List<Notes> {
                            new Notes {
                                Note = "Created by Endorsement"
                            }
                        };

                        var documentInfos = orderDoc.Content.Split(';');
                        uploadDoc.DocumentContent = new DocumentContent
                        {
                            ContentType = documentInfos[0].Replace("Data:", "").Replace("data:", ""),
                            FileContext = documentInfos[1].Replace("Base64,", "").Replace("base64,", ""),
                            FileName = orderDoc.Name.Contains('.') ? orderDoc.Name : orderDoc.Name + "." + documentInfos[0].Split('/')[1]
                        };

                        var json = JsonSerializer.Serialize(uploadDoc);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        Uri uri = new Uri(StaticValues.ContractUrl + "document/uploadInstance");
                        var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri);
                        httpRequest.Content = content;

                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.AuthToken.Replace("Bearer ", ""));
                        client.DefaultRequestHeaders.Add("business_line", request.ToBusinessLine);
                        client.DefaultRequestHeaders.Add("ToUserReference", request.ToUserReference);
                        client.DefaultRequestHeaders.Add("ToCustomerNo", request.ToCustomerNo);

                        var result = await client.SendAsync(httpRequest);
                        var responseContent = await result.Content.ReadAsStringAsync();

                        if (result.IsSuccessStatusCode)
                        {
                            Log.ForContext("ContractInstanceId", contractInstanceId)
                            .ForContext("UploadedDocument", json)
                            .ForContext("HttpResponseStatus", result.StatusCode)
                            .Information($"UploadContractDocumentInstanceCommand Document Uploaded.");
                        }
                        else
                        {
                            Log.ForContext("ContractInstanceId", contractInstanceId)
                            .ForContext("UploadedDocument", json)
                            .ForContext("HttpResponseStatus", result.StatusCode)
                            .Error($"UploadContractDocumentInstanceCommand Document Upload Error. Content: " + responseContent);
                            throw new Exception("Instance Request Error. Status Code: " + result.StatusCode);
                        }
                    }
                }
            }
            else if (formSource == "formio")
            {
                foreach (var orderDoc in orderDocuments)
                {
                    if (orderDoc.Type != "PlainText")
                    {
                        var currentMap = currentContract.Value.FirstOrDefault(x => x.EndorsementCode == orderDoc.Name.Replace(".pdf", ""));

                        ContractDocumentInstanceModel instanceDoc = new ContractDocumentInstanceModel
                        {
                            ContractCode = currentContract.Key,
                            ContractInstanceId = contractInstanceId,
                        };

                        instanceDoc.DocumentInstanceId = Guid.Parse(orderDoc.RenderId);
                        instanceDoc.DocumentCode = currentMap.DocumentCode;
                        instanceDoc.DocumentVersion = currentMap.DocumentVersion;
                        instanceDoc.RenderId = orderDoc.RenderId;
                        instanceDoc.Notes = new List<Notes> {
                            new Notes {
                                Note = "Created by Endorsement"
                            }
                        };

                        var documentInfos = orderDoc.Content.Split(';');
                        instanceDoc.DocumentContent = new DocumentContent
                        {
                            ContentType = documentInfos[0].Replace("Data:", "").Replace("data:", ""),
                            FileContext = documentInfos[1].Replace("Base64,", "").Replace("base64,", ""),
                            FileName = orderDoc.Name.Contains('.') ? orderDoc.Name : orderDoc.Name + "." + documentInfos[0].Split('/')[1]
                        };

                        var json = JsonSerializer.Serialize(instanceDoc);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        Uri uri = new Uri(StaticValues.ContractUrl + "document/instance");
                        var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri);
                        httpRequest.Content = content;

                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.AuthToken.Replace("Bearer ", ""));
                        client.DefaultRequestHeaders.Add("business_line", request.ToBusinessLine);
                        client.DefaultRequestHeaders.Add("ToUserReference", request.ToUserReference);
                        client.DefaultRequestHeaders.Add("ToCustomerNo", request.ToCustomerNo);

                        var result = await client.SendAsync(httpRequest);
                        var responseContent = await result.Content.ReadAsStringAsync();

                        if (result.IsSuccessStatusCode)
                        {
                            Log.ForContext("ContractInstanceId", contractInstanceId)
                            .ForContext("UploadedDocument", json)
                            .ForContext("HttpResponseStatus", result.StatusCode)
                            .Information($"UploadContractDocumentInstanceCommand Document Render Instance.");
                        }
                        else
                        {
                            Log.ForContext("ContractInstanceId", contractInstanceId)
                            .ForContext("UploadedDocument", json)
                            .ForContext("HttpResponseStatus", result.StatusCode)
                            .Error($"UploadContractDocumentInstanceCommand Document Render Instance Error. Content: " + responseContent);
                            throw new Exception("Instance Request Error. Status Code: " + result.StatusCode);
                        }
                    }
                }
            }

            var response = new UploadContractDocumentInstanceResponse
            {
                ContractInstanceId = contractInstanceId,
                ContractCode = currentContract.Key,
                Language = "tr-TR"
            };

            return Response<UploadContractDocumentInstanceResponse>.Success(response, 200);
        }
    }

    public class UploadContractDocumentInstanceModel
    {
        public string ContractCode { get; set; }
        public Guid ContractInstanceId { get; set; }
        public Guid DocumentInstanceId { get; set; }
        public string DocumentCode { get; set; }
        public string DocumentVersion { get; set; }
        public DocumentContent DocumentContent { get; set; }
        public List<InstanceMetadata> InstanceMetadata { get; set; }
        public List<Notes> Notes { get; set; }
    }

    public class ContractDocumentInstanceModel
    {
        public string ContractCode { get; set; }
        public Guid ContractInstanceId { get; set; }
        public Guid DocumentInstanceId { get; set; }
        public string DocumentCode { get; set; }
        public string DocumentVersion { get; set; }
        public string RenderId { get; set; }
        public DocumentContent DocumentContent { get; set; }
        public List<InstanceMetadata> InstanceMetadata { get; set; }
        public List<Notes> Notes { get; set; }
    }

    public class DocumentContent
    {
        public string ContentType { get; set; }
        public string FileContext { get; set; }
        public string FileName { get; set; }
    }

    public class InstanceMetadata
    {
        public string Code { get; set; }
        public string Data { get; set; }
    }

    public class Notes
    {
        public string Note { get; set; }
    }
}