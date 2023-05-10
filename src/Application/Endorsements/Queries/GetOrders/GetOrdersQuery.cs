using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Endorsements.Queries.GetOrders
{
    public class GetOrdersQuery : IRequest<Response<OrderItem[]>>
    {
        public long Approver { get; set; }
        public long Customer { get; set; }
        public long[] Submitter { get; set; }
        public long Status { get; set; }
        public string ReferenceProcess { get; set; }
        public Guid ReferenceId { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }

    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, Response<OrderItem[]>>
    {
        private IApplicationDbContext _context;

        public GetOrdersQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<OrderItem[]>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var orderItems = _context.Orders.Select(x => new OrderItem
            {
                Id = Guid.Parse(x.OrderId)
            });
            return Response<OrderItem[]>.Success(orderItems.ToArray(), 200);
        }
    }
}
