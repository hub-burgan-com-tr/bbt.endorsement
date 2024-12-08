using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Worker.App.Application.Workers.Commands.UploadContractDocumentInstances
{
    public class UploadContractDocumentInstanceCommand : IRequest<Response<UploadContractDocumentInstanceResponse>>
    {
        public string OrderId { get; set; }
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
            Guid contractInstanceId = Guid.NewGuid();
            var orderDocuments = _context.Documents.Where(x => x.OrderId == request.OrderId).ToList();
            var documentNames = orderDocuments.Select(x => x.Name.Replace(".pdf", "")).ToList();
            var maps = _context.ContractMaps.Where(x => documentNames.Contains(x.EndorsementCode) && x.RequiresFullMatch).GroupBy(x => x.ContractCode).ToDictionary(x => x.Key, x => x.ToList()); //ASK: serbest döküman için ContractCode boş bırakma?
            var currentContract = new KeyValuePair<string, List<ContractMap>>();

            foreach (var contract in maps)
            {
                var fullMatchList = contract.Value.Select(x => x.EndorsementCode).ToList();
                bool areEqual = new HashSet<string>(fullMatchList).SetEquals(documentNames);
                if (areEqual)
                {
                    currentContract = contract;
                    break;
                }
            }

            if (String.IsNullOrEmpty(currentContract.Key))
                currentContract = _context.ContractMaps.Where(x => documentNames.Contains(x.EndorsementCode) && !x.RequiresFullMatch).GroupBy(x => x.ContractCode).ToDictionary(x => x.Key, x => x.ToList()).FirstOrDefault();

            if (String.IsNullOrEmpty(currentContract.Key))
            {
                throw new Exception("ContractCode not found!");
            }

            foreach (var orderDoc in orderDocuments)
            {
                if (orderDoc.Type != "PlainText")
                {
                    var currentMap = currentContract.Value.FirstOrDefault(x => x.EndorsementCode == orderDoc.Name);
                    UploadContractDocumentInstanceModel uploadDoc = new UploadContractDocumentInstanceModel
                    {
                        ContractCode = currentContract.Key,
                        ContractInstanceId = contractInstanceId,
                    };

                    uploadDoc.DocumentInstanceId = Guid.NewGuid();
                    uploadDoc.DocumentCode = currentMap.DocumentCode;
                    uploadDoc.DocumentVersion = currentMap.DocumentVersion;

                    var documentInfos = orderDoc.Content.Split(';');
                    uploadDoc.DocumentContent = new DocumentContent
                    {
                        ContentType = documentInfos[0].Replace("Data:", ""),
                        FileContext = documentInfos[1].Replace("Base64,", ""),
                        FileName = orderDoc.Name.Contains('.') ? orderDoc.Name : orderDoc.Name + "." + documentInfos[0].Split('/')[1]
                    };
                }
            }

            var response = new UploadContractDocumentInstanceResponse
            {
                ContractInstanceId = contractInstanceId,
                ContractCode = currentContract.Key
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
        //public Notes Notes { get; set; }
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
}