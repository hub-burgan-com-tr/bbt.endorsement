using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models;
using Domain.Entities;
using MediatR;

namespace Application.Workers.Commands.SaveEntities
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
