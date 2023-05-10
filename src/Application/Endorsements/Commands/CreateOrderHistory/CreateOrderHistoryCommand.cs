using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Endorsements.Commands.CreateOrderHistory
{
    public class CreateOrderHistoryCommand : IRequest<Response<bool>>
    {
        public string OrderId { get; set; }
        public string DocumentId { get; set; }
        public string State { get; set; }
        public string Description { get; set; }

        public class OrderHistoryCommandHandler : IRequestHandler<CreateOrderHistoryCommand, Response<bool>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IDateTime _dateTime;

            public OrderHistoryCommandHandler(IApplicationDbContext context, IDateTime dateTime)
            {
                _context = context;
                _dateTime = dateTime;
            }

            public async Task<Response<bool>> Handle(CreateOrderHistoryCommand request, CancellationToken cancellationToken)
            {
                _context.OrderHistories.Add(new Domain.Entities.OrderHistory
                {
                    OrderHistoryId = Guid.NewGuid().ToString(),
                    OrderId = request.OrderId,
                    DocumentId = request.DocumentId,
                    State = request.State,
                    Description = request.Description,
                    Created = _dateTime.Now
                });
                var result = _context.SaveChanges();
                return Response<bool>.Success(result > 0 ? true : false, 200);
            }
        }

    }
}