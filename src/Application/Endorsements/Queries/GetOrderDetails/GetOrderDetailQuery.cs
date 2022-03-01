using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Endorsements.Queries.GetOrderDetails
{
    public class GetOrderDetailQuery : IRequest<Response<OrderDetail>>
    {
        public Guid Id { get; set; }
    }

    public class GetOrderDetailQueryHandler : IRequestHandler<GetOrderDetailQuery, Response<OrderDetail>>
    {
        private IApplicationDbContext _context;

        public GetOrderDetailQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<OrderDetail>> Handle(GetOrderDetailQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<OrderDetail>();
            return response;
        }
    }
}
