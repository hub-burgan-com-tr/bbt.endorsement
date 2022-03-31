using MediatR;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Coomon.Models;
using Worker.App.Domain.Entities;

namespace Worker.AppApplication.Endorsements.Commands.CreateOrderHistory
{
    public class CreateOrderHistoryCommand : IRequest<Response<bool>>
    {
        public string OrderId { get; set; }
        public string DocumentId { get; set; }
        public string State { get; set; }
        public string Name { get; set; }

        public class OrderHistoryCommandHandler : IRequestHandler<CreateOrderHistoryCommand, Response<bool>>
        {
            private readonly IApplicationDbContext _context;

            public OrderHistoryCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Response<bool>> Handle(CreateOrderHistoryCommand request, CancellationToken cancellationToken)
            {
                _context.OrderHistories.Add(new OrderHistory
                {
                    OrderHistoryId=Guid.NewGuid().ToString(),
                    OrderId=request.OrderId,
                    DocumentId=request.DocumentId,
                    State=request.State,
                    Name=request.Name,
                    Created=DateTime.Now
                });
                 var result=  _context.SaveChanges();
                return Response<bool>.Success(result>0?true:false, 200);
            }
        }

    }
}
