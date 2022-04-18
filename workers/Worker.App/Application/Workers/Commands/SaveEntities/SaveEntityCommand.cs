using MediatR;
using Worker.App.Application.Common.Models;
using Worker.App.Domain.Entities;
using Worker.App.Models;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Domain.Enums;

namespace Worker.App.Application.Workers.Commands.SaveEntities
{
    public class SaveEntityCommand : IRequest<Response<SaveEntityResponse>>
    {
        public ContractModel Model { get; set; }   
        public long ProcessInstanceKey { get; set; }
    }

    public class SaveEntityCommandHandler : IRequestHandler<SaveEntityCommand, Response<SaveEntityResponse>>
    {
        private IApplicationDbContext _context;
        private ISaveEntityService _saveEntityService;
        private IDateTime _dateTime;

        public SaveEntityCommandHandler(IApplicationDbContext context, ISaveEntityService saveEntityService, IDateTime dateTime)
        {
            _context = context;
            _saveEntityService = saveEntityService;
            _dateTime = dateTime;
        }

        public async Task<Response<SaveEntityResponse>> Handle(SaveEntityCommand request, CancellationToken cancellationToken)
        {
            var orderId = "";
            if(request.Model.FormType == Form.Order)
                orderId = OrderCreate(request.Model.StartRequest, request.ProcessInstanceKey);
            else if (request.Model.FormType == Form.FormOrder)
                orderId = FormOrderCreate(request.Model.StartFormRequest, request.ProcessInstanceKey);

            var documentList = _context.Documents.Where(x => x.OrderId == orderId);
            var saveEntityDocuments = new List<SaveEntityDocumentResponse>();
            foreach (var item in documentList)
                saveEntityDocuments.Add(new SaveEntityDocumentResponse { DocumentId = item.DocumentId, Name = item.Name });
            var response =  new SaveEntityResponse { OrderId = orderId, Documents = saveEntityDocuments };

            return Response<SaveEntityResponse>.Success(response, 200);
        }

        private string FormOrderCreate(StartFormRequest startFormRequest, long processInstanceKey)
        {
            if (startFormRequest == null) return null;

            var documents = new List<Document>();

            var formDefinition = _saveEntityService.GetFormDefinition(startFormRequest.FormId).Result;
            var config = new Config
            {
                ExpireInMinutes = formDefinition.ExpireInMinutes,
                MaxRetryCount = formDefinition.MaxRetryCount,
                RetryFrequence = formDefinition.RetryFrequence,
            };

            var actions = new List<DocumentAction>();

            foreach (var action in formDefinition.Actions)
            {
                actions.Add(new DocumentAction
                {
                    DocumentActionId = Guid.NewGuid().ToString(),
                    Created = _dateTime.Now,
                    Choice = action.Choice,
                    Title = action.Title,
                    Type = action.Choice==(int)ActionType.Approve ? ActionType.Approve.ToString() : ActionType.Reject.ToString()
                });
            }

            documents.Add(new Document
            {
                DocumentId = Guid.NewGuid().ToString(),
                Content = startFormRequest.Content,
                Name = startFormRequest.Title,
                Type = formDefinition.Type.ToString(),
                FileType=formDefinition.Type.ToString(),
                MimeType=formDefinition.Type.ToString()==ContentType.HTML.ToString()? "text/html": "application/pdf",
                Created = _dateTime.Now,
                DocumentActions = actions,
                FormDefinitionId = startFormRequest.FormId
            });

            var order = new Order
            {
                OrderId = startFormRequest.Id.ToString(),
                ProcessInstanceKey = processInstanceKey,
                State = OrderState.Pending.ToString(),
                Title = startFormRequest.Title,
                Created = _dateTime.Now,
                Config = config,
                CustomerId = GetCustomerId(startFormRequest.Approver),
                Reference = new Reference
                {
                    ProcessNo = startFormRequest.Reference.ProcessNo,
                    Created = _dateTime.Now,
                    Process = startFormRequest.Reference.Process,
                    State = startFormRequest.Reference.State,
                },
                Documents = documents,
            };
            var entity = _context.Orders.Add(order).Entity;
            _context.SaveChanges();

            return entity.OrderId;
        }

        private string GetCustomerId(OrderApprover approver)
        {
            var customerId = _saveEntityService.GetCustomerAsync(approver.CitizenshipNumber).Result;
            if (customerId == null)
                customerId = _saveEntityService.CustomerSaveAsync(approver).Result;
            return customerId;
        }

        private string OrderCreate(StartRequest startRequest, long processInstanceKey)
        {
            if (startRequest == null) return null;

            var documents = new List<Document>();
            foreach (var item in startRequest.Documents)
            {
                var actions = new List<DocumentAction>();
                if (item.Actions != null)
                {
                    foreach (var action in item.Actions)
                    {
                        actions.Add(new DocumentAction
                        {
                            DocumentActionId = Guid.NewGuid().ToString(),
                            Created = _dateTime.Now,
                            Choice = action.Choice,
                            Title = action.Title,
                            Type = action.Choice == (int)ActionType.Approve ? ActionType.Approve.ToString() : ActionType.Reject.ToString()
                        });
                    }
                }

                documents.Add(new Document
                {
                    DocumentId = Guid.NewGuid().ToString(),
                    Content = item.Content,
                    Name =item.Title,
                    Type = item.Type.ToString(),
                    FileType =item.Type.ToString()==ContentType.PlainText.ToString()?FileType.PlainText.ToString():GetFileType(item.FileType),
                    MimeType=item.FileType,
                    Created = _dateTime.Now,
                    DocumentActions = actions
                });
            }

            var config = new Config();
            if (startRequest.Config != null)
            {
                config.MaxRetryCount = startRequest.Config.MaxRetryCount;
                config.RetryFrequence = startRequest.Config.RetryFrequence;
                config.ExpireInMinutes = startRequest.Config.ExpireInMinutes;
            }

            var order = new Order
            {
                OrderId = startRequest.Id.ToString(),
                ProcessInstanceKey = processInstanceKey,
                State = OrderState.Pending.ToString(),
                Title = startRequest.Title,
                Created = _dateTime.Now,
                Config = config,
                CustomerId = GetCustomerId(startRequest.Approver),
                Reference = new Reference
                {
                    ProcessNo = startRequest.Reference.ProcessNo,
                    Created = _dateTime.Now,
                    Process = startRequest.Reference.Process,
                    State = startRequest.Reference.State,
                },
                Documents = documents,
            };
            var entity = _context.Orders.Add(order).Entity;
            _context.SaveChanges();

            return entity.OrderId;
        }

        private string GetFileType(string fileType)
        {
            if (fileType.Contains("image"))
            {
                return FileType.Image.ToString();
            }
            else if (fileType.Contains("pdf"))
            {
                return FileType.PDF.ToString();

            }
            if (fileType.Contains("html"))
            {
                return FileType.HTML.ToString();

            }
            else
            {
                return FileType.File.ToString();

            }

        }
        public enum FileType
        { Image = 1, PDF = 2, HTML = 3, File = 4, PlainText=5 }
        public enum ContentType { File = 1, PlainText = 2, HTML = 3, PDF = 4, }

    }
}
