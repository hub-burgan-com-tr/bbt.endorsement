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
                documents.Add(new Document
                {
                    Content = item.Content,
                    Name = item.Title,
                    Type = item.Type.ToString(),
                });
            }
            var order = new Order
            {
                OrderId = data.Id.ToString(),
                Title = data.Title,
                Config = new Config
                {
                    MaxRetryCount = data.Config.MaxRetryCount,
                    RetryFrequence = data.Config.RetryFrequence,
                    ExpireInMinutes = data.Config.ExpireInMinutes
                },
                Reference = new Reference
                {
                    ProcessNo = data.Reference.Process
                },
                Documents = documents,
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            return Response<SaveEntityResponse>.Success(200);
        }
    }
}
