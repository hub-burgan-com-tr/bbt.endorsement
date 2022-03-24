using MediatR;
using Worker.App.Application.Coomon.Models;
using Worker.App.Domain.Entities;
using Worker.App.Models;
using Worker.App.Application.Common.Interfaces;

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
            var data = request.Model.StartRequest;

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
                            Type = action.IsDefault?ActionType.Approve.ToString():ActionType.Reject.ToString()
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
    }
}
