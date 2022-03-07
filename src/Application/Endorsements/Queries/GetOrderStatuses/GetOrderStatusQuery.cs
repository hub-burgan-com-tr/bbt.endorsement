using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Endorsements.Queries.GetOrderStatuses
{
    public class GetOrderStatusQuery : IRequest<Response<string>>
    {
        public Guid Id { get; set; }
    }

    public class GetOrderStatusQueryHandler : IRequestHandler<GetOrderStatusQuery, Response<string>>
    {
        private IApplicationDbContext _context;

        public GetOrderStatusQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<string>> Handle(GetOrderStatusQuery request, CancellationToken cancellationToken)
        {
            return Response<string>.Success("", 200);
        }
    }
}
