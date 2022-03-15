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
            var order = new Order
            {
                OrderId = data.Id.ToString(),
                Title = data.Title
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            return Response<SaveEntityResponse>.Success(200);
        }
    }
}
