using Application.Common.Models;
using MediatR;

namespace Application.Endorsements.Queries.GetOrders
{
    public class GetOrdersCommand : IRequest<Response<OrderItem[]>>
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

    public class GetOrdersCommandHandler : IRequestHandler<GetOrdersCommand, Response<OrderItem[]>>
    {
        public async Task<Response<OrderItem[]>> Handle(GetOrdersCommand request, CancellationToken cancellationToken)
        {
            var orderItems = new List<OrderItem>();
            return Response<OrderItem[]>.Success(orderItems.ToArray(), 200);
        }
    }
}
