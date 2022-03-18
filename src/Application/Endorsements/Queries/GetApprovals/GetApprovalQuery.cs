using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Endorsements.Queries.GetApprovals
{
    public class GetApprovalQuery : IRequest<Response<PaginatedList<GetApprovalDto>>>
    {/// <summary>
    /// Instance Id
    /// </summary>
        public string OrderId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    /// <summary>
    /// Onayimdakiler Listesi
    /// </summary>
    public class GetApprovalQueryHandler : IRequestHandler<GetApprovalQuery, Response<PaginatedList<GetApprovalDto>>>
    {
        private IApplicationDbContext _context;

        public GetApprovalQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<PaginatedList<GetApprovalDto>>> Handle(GetApprovalQuery request, CancellationToken cancellationToken)
        {
            var list = await _context.Orders.Where(x => x.OrderId == request.OrderId).Include(x => x.Documents).OrderBy(x=>x.Title).ThenByDescending(x=>x.Created).Select(x => new GetApprovalDto { OrderId = x.OrderId, Title = x.Title, IsDocument = x.Documents.Any() }).PaginatedListAsync(request.PageNumber, request.PageSize);
            return Response<PaginatedList<GetApprovalDto>>.Success(list, 200);
        }
    }
}
