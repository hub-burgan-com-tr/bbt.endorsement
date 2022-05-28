using Dms.Integration.Infrastructure.Document;
using Dms.Integration.Infrastructure.DocumentServices;
using Dms.Integration.Infrastructure.Enums;
using Dms.Integration.Infrastructure.Models;
using Dms.Integration.Infrastructure.Services;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;

namespace Worker.App.Application.Documents.Commands.CreateDMSDocuments;

public class CreateDMSDocumentCommand : IRequest<Response<string>>
{
    public ApproveOrderDocument Document { get; set; }
    public string InstanceId { get; set; }
}

public class CreateDMSDocumentCommandHandler : IRequestHandler<CreateDMSDocumentCommand, Response<string>>
{
    private IDocumentService _documentService = null!;
    private IApplicationDbContext _context;

    public CreateDMSDocumentCommandHandler(IDocumentService documentService, IApplicationDbContext context)
    {
        _documentService = documentService;
        _context = context;
    }

    public async Task<Response<string>> Handle(CreateDMSDocumentCommand request, CancellationToken cancellationToken)
    {
        var order = _context.Orders.Include(x=> x.Person).Include(x => x.Customer).FirstOrDefault(x => x.OrderId == request.InstanceId.ToString());
        var document = _context.Documents.FirstOrDefault(x => x.OrderId == request.InstanceId.ToString() && x.DocumentId == request.Document.DocumentId);
        var person = order.Person;        
        var customer = order.Customer;

        try
        {
            if(document != null)
            {
                var documentInsuranceTypes = _context.DocumentInsuranceTypes
                    .Where(x => x.DocumentId == document.DocumentId)
                    .Select(x => new
                    {
                        DmsReferenceId = x.Parameter.DmsReferenceId
                    }).Distinct();

                foreach (var documentInsuranceType in documentInsuranceTypes)
                {
                    var channelReferenceId = "";
                    var version = "";
                    var branchCode = "2000";
                    var bhsOrderNo = "";

                    var dmsPerson = new DmsPerson
                    {
                        CitizenshipNumber = person.CitizenshipNumber,
                        ClientNumber = person.ClientNumber,
                        PersonId = person.PersonId,
                        FirstName = person.FirstName,
                        LastName = person.LastName,
                    };

                    var dmsDocument = new DmsDocument
                    {
                        Definition = new DocumentDefinition
                        {
                            DmsReferenceId = documentInsuranceType.DmsReferenceId.ToString(),
                            Key = DocumentDefinitionType.None
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

                    dmsDocument.OwnerActionType = DocumentActionType.OnlineSigned;
                    DocumentInfo documentInfo = new BhsDocument(dmsDocument, dmsPerson, channelReferenceId, customer.ClientNumber, customer.CitizenshipNumber.ToString(), branchCode, bhsOrderNo, version)
                    {
                        DmsPrefix = "InternetBankaciligi"
                    };

                    var dmsRefId = _documentService.CreateDMSDocument(documentInfo);

                    if (dmsRefId != null)
                    {
                        var documentUpdate = _context.Documents.FirstOrDefault(x => x.DocumentId == document.DocumentId);
                        if (documentUpdate != null)
                        {
                            documentUpdate.DmsReferenceId = dmsRefId;
                            _context.Documents.Update(documentUpdate);
                            _context.SaveChanges();
                        }
                    }
                }              
            }
        }
        catch (Exception ex)
        {
            //        logger.LogError(EventIdConstants.SendDocumentToDmsException, "Döküman DMS'e atılırken bir hata oluştu.", ex);
        }

        return Response<string>.Success(200);
    }
}