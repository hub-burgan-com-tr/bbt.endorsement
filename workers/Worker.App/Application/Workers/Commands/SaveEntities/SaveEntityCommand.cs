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

        public Form FormType { get; set; }
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
            if(request.FormType == Form.Order)
            {
               return OrderCreate(request.Model.StartRequest);
            }
            else if (request.FormType == Form.FormOrder)
            {
                return FormOrderCreate(request.Model.StartFormRequest);
            }

            var data = request.Model.StartRequest;

            if (data == null) return null;

            var documents = new List<Document>();
            foreach (var item in data.Documents)
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
                            IsDefault = action.IsDefault,
                            Title = action.Title,
                            Type = action.IsDefault ? ActionType.Approve.ToString() : ActionType.Reject.ToString()
                        });
                    }
                }

                documents.Add(new Document
                {
                    DocumentId = Guid.NewGuid().ToString(),
                    Content = item.Content,
                    Name = item.Title,
                    Type = item.Type.ToString(),
                    Created = _dateTime.Now,
                    DocumentActions = actions
                });
            }

            var config = new Config();
            if(data.Config != null)
            {
                config.MaxRetryCount = data.Config.MaxRetryCount;
                config.RetryFrequence = data.Config.RetryFrequence;
                config.ExpireInMinutes = data.Config.ExpireInMinutes;
            }
            else
            {
                config.MaxRetryCount = 3;
                config.RetryFrequence = "4";
                config.ExpireInMinutes = 60;
            }

            var order = new Order
            {
                OrderId = data.Id.ToString(),
                Title = data.Title,
                Created = _dateTime.Now,
                Config = config,
                Reference = new Reference
                {
                    ProcessNo = data.Reference.ProcessNo,
                    Created = _dateTime.Now,
                    Process = data.Reference.Process,
                    State = data.Reference.State,
                },
                Documents = documents,
            };
            var response = _context.Orders.Add(order).Entity;
            _context.SaveChanges();

            return Response<SaveEntityResponse>.Success(new SaveEntityResponse { OrderId = response.OrderId }, 200);
        }

        private Response<SaveEntityResponse> FormOrderCreate(StartFormRequest startFormRequest)
        {
            if (startFormRequest == null) return null;

            var documents = new List<Document>();


            var actions = new List<DocumentAction>();

            //foreach (var action in item.Actions)
            //{
            //    actions.Add(new DocumentAction
            //    {
            //        DocumentActionId = Guid.NewGuid().ToString(),
            //        Created = _dateTime.Now,
            //        IsDefault = action.IsDefault,
            //        Title = action.Title,
            //        Type = action.IsDefault ? ActionType.Approve.ToString() : ActionType.Reject.ToString()
            //    });
            //}

            documents.Add(new Document
            {
                DocumentId = Guid.NewGuid().ToString(),
                Content = startFormRequest.Content,
                Name = startFormRequest.Title,
                //Type = startFormRequest.Type.ToString(),
                Created = _dateTime.Now,
                DocumentActions = actions
            });

            var config = new Config();

            var order = new Order
            {
                OrderId = startFormRequest.Id.ToString(),
                Title = startFormRequest.Title,
                Created = _dateTime.Now,
                Config = config,
                Reference = new Reference
                {
                    ProcessNo = startFormRequest.Reference.ProcessNo,
                    Created = _dateTime.Now,
                    Process = startFormRequest.Reference.Process,
                    State = startFormRequest.Reference.State,
                },
                Documents = documents,
            };
            var response = _context.Orders.Add(order).Entity;
            _context.SaveChanges();

            return Response<SaveEntityResponse>.Success(new SaveEntityResponse { OrderId = response.OrderId }, 200);
        }

        private Response<SaveEntityResponse> OrderCreate(StartRequest startRequest)
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
                            IsDefault = action.IsDefault,
                            Title = action.Title,
                            Type = action.IsDefault ? ActionType.Approve.ToString() : ActionType.Reject.ToString()
                        });
                    }
                }

                documents.Add(new Document
                {
                    DocumentId = Guid.NewGuid().ToString(),
                    Content = item.Content,
                    Name = item.Title,
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
                Reference = new Reference
                {
                    ProcessNo = startRequest.Reference.ProcessNo,
                    Created = _dateTime.Now,
                    Process = startRequest.Reference.Process,
                    State = startRequest.Reference.State,
                },
                Documents = documents,
            };
            var response = _context.Orders.Add(order).Entity;
            _context.SaveChanges();

            return Response<SaveEntityResponse>.Success(new SaveEntityResponse { OrderId = response.OrderId }, 200);
        }
    }
}
