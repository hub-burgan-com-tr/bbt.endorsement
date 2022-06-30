using Dms.Integration.Infrastructure.Document;
using Dms.Integration.Infrastructure.DocumentServices;
using Dms.Integration.Infrastructure.Enums;
using Dms.Integration.Infrastructure.Models;
using Dms.Integration.Infrastructure.Services;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;

namespace Worker.App.Application.Documents.Commands.CreateDMSDocuments;

public class CreateDMSDocumentCommand : IRequest<Response<List<CreateDMSDocumentResponse>>>
{
    public string InstanceId { get; set; }
}

public class CreateDMSDocumentCommandHandler : IRequestHandler<CreateDMSDocumentCommand, Response<List<CreateDMSDocumentResponse>>>
{
    private IDocumentService _documentService = null!;
    private IApplicationDbContext _context;

    public CreateDMSDocumentCommandHandler(IDocumentService documentService, IApplicationDbContext context)
    {
        _documentService = documentService;
        _context = context;
    }

    public async Task<Response<List<CreateDMSDocumentResponse>>> Handle(CreateDMSDocumentCommand request, CancellationToken cancellationToken)
    {
        var order = _context.Orders.Include(x => x.Person).Include(x => x.Customer).FirstOrDefault(x => x.OrderId == request.InstanceId.ToString());
        var documents = _context.Documents.Where(x => x.OrderId == request.InstanceId.ToString());
        var person = order.Person;
        var customer = order.Customer;

        var dmses= new List<CreateDMSDocumentResponse>();
        if(order.State != OrderState.Approve.ToString())
            return Response<List<CreateDMSDocumentResponse>>.Success(dmses, 200);

        foreach (var document in documents)
        {
            var documentInsuranceTypes = _context.DocumentInsuranceTypes
                .Where(x => x.DocumentId == document.DocumentId)
                .Select(x => new
                {
                    DmsReferenceId = x.Parameter.DmsReferenceId,
                    DmsReferenceKey = x.Parameter.DmsReferenceKey,
                    DmsReferenceName = x.Parameter.DmsReferenceName,
                }).ToList().Distinct();

            if (documentInsuranceTypes.Any())
            {
                foreach (var documentInsuranceType in documentInsuranceTypes)
                {
                    var dmsReferenceId = documentInsuranceType.DmsReferenceId.ToString();
                    var dmsReferenceKey = documentInsuranceType.DmsReferenceId;
                    var dmsReferenceName = documentInsuranceType.DmsReferenceName;
                    var dms = CreateDMSDocumentSend(document, customer, dmsReferenceId, dmsReferenceKey, dmsReferenceName, request.InstanceId);
                    dmses.Add(dms);
                }
            }
            else
            {
                var dmsReferenceId = "1572";
                var dmsReferenceKey = 1200;
                var dmsReferenceName = "Diğer Elementer Poliçesi";
                var dms = CreateDMSDocumentSend(document, customer, dmsReferenceId, dmsReferenceKey, dmsReferenceName,  request.InstanceId);
                dmses.Add(dms);
            }
        }

        return Response<List<CreateDMSDocumentResponse>>.Success(dmses, 200);
    }

    private CreateDMSDocumentResponse CreateDMSDocumentSend(Domain.Entities.Document document, Domain.Entities.Customer customer, string dmsReferenceId, int? dmsReferenceKey, string dmsReferenceName, string instanceId)
    {
        var response = new CreateDMSDocumentResponse();

        var channelReferenceId = "";
        var version = "";
        var branchCode = "2000";
        var bhsOrderNo = "";

        var dmsPerson = new DmsPerson
        {
            CitizenshipNumber = customer.CitizenshipNumber,
            ClientNumber = customer.CustomerNumber,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
        };

        var dmsDocument = new DmsDocument
        {
            Definition = new DocumentDefinition
            {
                DmsReferenceId = dmsReferenceId,
                Key = dmsReferenceName, //(DocumentDefinitionType)Enum.Parse(typeof(DocumentDefinitionType), dmsReferenceKey)
            },
            Contents = new List<DocumentContent>()
        };

        var data = document.Content.Split(',');
        var content = data[1];
        var contentBtye = Convert.FromBase64String(content);
        dmsDocument.Contents.Add(new DocumentContent
        {
            Content = contentBtye,
            MimeType = document.MimeType,
        });

        dmsDocument.OwnerActionType = DocumentActionType.Checked;
        DocumentInfo documentInfo = new BhsDocument(dmsDocument, dmsPerson, channelReferenceId, customer.CustomerNumber, customer.CitizenshipNumber.ToString(), branchCode, bhsOrderNo, version)
        {
            DmsPrefix = "InternetBankaciligi"
        };

        var dmsRefId = _documentService.CreateDMSDocument(documentInfo);

        if (!string.IsNullOrEmpty(dmsRefId))
        {
            response = new CreateDMSDocumentResponse
            {
                DmsRefId = dmsRefId,
                DmsReferenceKey = dmsReferenceKey,
                DmsReferenceName = dmsReferenceName
            };

            var documentDms = _context.DocumentDmses
                                        .FirstOrDefault(x => x.Document.OrderId == instanceId &&
                                                             x.DocumentId == document.DocumentId &&
                                                             x.DmsReferenceId == dmsRefId);
            if (documentDms == null)
            {
                _context.DocumentDmses.Add(new Domain.Entities.DocumentDms
                {
                    DocumentDmsId = Guid.NewGuid().ToString(),
                    DocumentId = document.DocumentId,
                    DmsReferenceId = dmsRefId,
                    DmsReferenceKey = dmsReferenceKey,
                    DmsReferenceName= dmsReferenceName
                });
                _context.SaveChanges();
            }
        }

        return response;
    }
}