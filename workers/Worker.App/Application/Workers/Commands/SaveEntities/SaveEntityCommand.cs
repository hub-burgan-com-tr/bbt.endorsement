using MediatR;
using Worker.App.Application.Coomon.Models;
using Worker.App.Domain.Entities;
using Worker.App.Models;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Domain.Enums;
using static Worker.App.Models.StartRequest;

namespace Worker.App.Application.Workers.Commands.SaveEntities
{
    public class SaveEntityCommand : IRequest<Response<SaveEntityResponse>>
    {
        public ContractModel Model { get; set; }    
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
            var response = new SaveEntityResponse();
            if(request.Model.FormType == Form.Order)
            {
                response= OrderCreate(request.Model.StartRequest);
            }
            else if (request.Model.FormType == Form.FormOrder)
            {
                response = FormOrderCreate(request.Model.StartFormRequest);
            }

            return Response<SaveEntityResponse>.Success(response, 200);
        }

        private SaveEntityResponse FormOrderCreate(StartFormRequest startFormRequest)
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
                Created = _dateTime.Now,
                DocumentActions = actions,
                FormDefinitionId = startFormRequest.FormId
            });

            var order = new Order
            {
                OrderId = startFormRequest.Id.ToString(),
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

            return new SaveEntityResponse { OrderId = entity.OrderId };
        }

        private string GetCustomerId(OrderApprover approver)
        {
            var customerId = _saveEntityService.GetCustomerAsync(approver.CitizenshipNumber).Result;
            if (customerId == null)
                customerId = _saveEntityService.CustomerSaveAsync(approver).Result;
            return customerId;
        }

        private SaveEntityResponse OrderCreate(StartRequest startRequest)
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
                            Type = action.Choice==(int)ActionType.Approve ? ActionType.Approve.ToString() : ActionType.Reject.ToString()
                        });
                    }
                }

                documents.Add(new Document
                {
                    DocumentId = Guid.NewGuid().ToString(),
                    Content = item.Content,
                    Name =item.Title,
                    Type = item.Type.ToString(),
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

            return new SaveEntityResponse { OrderId = entity.OrderId };
        }
    }
}
