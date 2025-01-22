using System.Text;
using System.Text.Json;
using MediatR;
using Domain.Entities;
using Worker.App.Application.Common.Models;
using Worker.App.Application.Common.Interfaces;
using System.Net.Http.Headers;
using Serilog;

namespace Worker.App.Application.Workers.Commands.StartFreeContractApprovals
{
    public class StartFreeContractApprovalCommand : IRequest<Response<StartFreeContractApprovalResponse>>
    {
        public string ContractCode { get; set; }
        public string ContractTitle { get; set; }
        public string ToUserReference { get; set; }
        public string ToCustomerNo { get; set; }
        public string ToLangCode { get; set; }
        public string SetTimeout { get; set; }
        public string OrderId { get; set; }
        public string ToBusinessLine { get; set; }
        public Guid ContractInstanceId { get; set; }
        public string AuthToken { get; set; }
    }

    public class StartFreeContractApprovalCommandHandler : IRequestHandler<StartFreeContractApprovalCommand, Response<StartFreeContractApprovalResponse>>
    {
        private readonly IApplicationDbContext _context;

        public StartFreeContractApprovalCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<StartFreeContractApprovalResponse>> Handle(StartFreeContractApprovalCommand request, CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(request.ContractCode))
            {
                var map = _context.ContractMaps.Where(x => x.EndorsementCode == request.ContractTitle).FirstOrDefault();
                request.ContractCode = map != null ? map.ContractCode : "";
                request.ToLangCode = map != null ? map.Language : "";
            }

            if (String.IsNullOrEmpty(request.ContractCode))
                throw new Exception("ContractCode can not be empty!");

            var contractDocumentList = _context.ContractMaps.Where(x => x.ContractCode == request.ContractCode).ToList();
            var currentDocuments = _context.ContractStarts.Where(x => x.ContractInstanceId == request.ContractInstanceId).Select(x => x.ContractDocuments).FirstOrDefault();
            var currentDocumentNames = currentDocuments.Split(';');

            List<object> decisionTableTags = new List<object>();
            foreach (var contractDocument in contractDocumentList)
            {
                decisionTableTags.Add(new {
                    setRequired = currentDocumentNames.Contains(contractDocument.EndorsementCode),
                    setDocument = contractDocument.DocumentCode
                });
            }

            var orderFreeDocuments = _context.Documents.Where(x => x.OrderId == request.OrderId && x.Type == "PlainText").ToList();
            List<object> freeDocuments = new List<object>();
            foreach (var orderDocument in orderFreeDocuments)
            {
                byte[] decodedBytes = Convert.FromBase64String(orderDocument.Content.Split(',')[1]);
                string decodedString = Encoding.UTF8.GetString(decodedBytes);

                var documentCode = _context.ContractMaps.Where(x => x.EndorsementCode == orderDocument.Name).Select(x => x.DocumentCode).FirstOrDefault();
                freeDocuments.Add(new
                {
                    DocumentCode = documentCode, //ASK: Her bir serbset için tanım yapılacak contract'a template free-document seçilecek.
                    DocumentContent = decodedString
                });
            }
            
            var client = new HttpClient();
            client.BaseAddress = new Uri(StaticValues.AmorphieWorkflowUrl);
            object reqObj = new
            {
                ContractCode = request.ContractCode,
                ToUserReference = request.ToUserReference,
                ToCustomerNo = request.ToCustomerNo,
                ToLangCode = request.ToLangCode,
                ContractTitle = request.ContractTitle,
                SetTimeout = request.SetTimeout,
                ToBusinessLine = request.ToBusinessLine,
                FreeDocuments = freeDocuments,
                DecisionTable = new {
                    Id = "ManageRequiredDocumentsDMN",
                    Tags = decisionTableTags
                }
            };

            var response = new StartFreeContractApprovalResponse
            {
                WorkflowId = request.ContractInstanceId
            };

            Uri uri = new Uri(StaticValues.AmorphieWorkflowUrl + "workflow/instance/" + response.WorkflowId.ToString() + "/transition/free-contract-approval-start"); //TO DO: Remove
            // Uri uri = new Uri(StaticValues.AmorphieWorkflowUrl + "instance/" + response.WorkflowId.ToString() + "/transition/free-contract-approval-start");
            var json = JsonSerializer.Serialize(reqObj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri);
            httpRequest.Content = content;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.AuthToken.Replace("Bearer ", ""));
            client.DefaultRequestHeaders.Add("User", "650c0ab5-7e1d-4d06-a7ce-f75e6857da68");
            client.DefaultRequestHeaders.Add("Behalf-Of-User", "650c0ab5-7e1d-4d06-a7ce-f75e6857da68");
            client.DefaultRequestHeaders.Add("ClientId", "IbWeb"); //TO DO: Remove
            
            var result = await client.SendAsync(httpRequest);
            var responseContent = await result.Content.ReadAsStringAsync();

            if (result.IsSuccessStatusCode)
            {
                Log.ForContext("ContractInstanceId", request.ContractInstanceId)
                .ForContext("FreeContractBody", json)
                .ForContext("HttpResponseStatus", result.StatusCode)
                .Information($"Free contract started.");
            }
            else
            {
                Log.ForContext("ContractInstanceId", request.ContractInstanceId)
                .ForContext("UploadedDocument", json)
                .ForContext("HttpResponseStatus", result.StatusCode)
                .Error($"StartFreeContract Error. Content: " + responseContent);
            }

            return Response<StartFreeContractApprovalResponse>.Success(response, 200);
        }
    }
}