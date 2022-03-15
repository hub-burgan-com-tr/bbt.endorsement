using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Endorsements.Queries.GetApprovals
{
    public class GetApprovalQuery : IRequest<Response<List<GetApprovalDto>>>
    {/// <summary>
    /// Instance Id
    /// </summary>
        public string OrderId { get; set; }
    }

    /// <summary>
    /// Onayimdakiler Listesi
    /// </summary>
    public class GetApprovalQueryHandler : IRequestHandler<GetApprovalQuery, Response<List<GetApprovalDto>>>
    {
        private IApplicationDbContext _context;

        public GetApprovalQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<GetApprovalDto>>> Handle(GetApprovalQuery request, CancellationToken cancellationToken)
        {
            var list = _context.Orders.Where(x => x.OrderId == request.OrderId).Include(x => x.Documents).Select(x => new GetApprovalDto { OrderId = x.OrderId, OrderName = x.Title, IsDocument = x.Documents.Any() }).ToList();
            return Response<List<GetApprovalDto>>.Success(list, 200);
        }
    }
}
