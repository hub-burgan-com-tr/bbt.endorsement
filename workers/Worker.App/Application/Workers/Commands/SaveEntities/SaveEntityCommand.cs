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

        public SaveEntityCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<SaveEntityResponse>> Handle(SaveEntityCommand request, CancellationToken cancellationToken)
        {
            var data = request.Model.StartRequest;

            var documents = new List<Document>();
            foreach (var item in data.Documents)
            {
                var actions = new List<Domain.Entities.Action>();
                foreach (var action in item.Actions)
                {
                    actions.Add(new Domain.Entities.Action
                    {
                        ActionId = Guid.NewGuid().ToString(),
                        Created = DateTime.Now,
                        IsDefault = action.IsDefault,
                        Title = action.Title,
                        Type = action.Type.ToString()
                    });
                }

                documents.Add(new Document
                {
                    DocumentId = Guid.NewGuid().ToString(),
                    Content = item.Content,
                    Name = item.Title,
                    Type = item.Type.ToString(),
                    Created = DateTime.Now,
                    Actions = actions
                });
            }
            var order = new Order
            {
                OrderId = data.Id.ToString(),
                Title = data.Title,
                Created = DateTime.Now,
                Config = new Config
                {
                    MaxRetryCount = data.Config.MaxRetryCount,
                    RetryFrequence = data.Config.RetryFrequence,
                    ExpireInMinutes = data.Config.ExpireInMinutes
                },
                Reference = new Reference
                {
                    ProcessNo = data.Reference.ProcessNo,
                    Created = DateTime.Now,
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
