using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Endorsements.Queries.GetWantApprovals
{
    public class GetWantApprovalQuery : IRequest<Response<PaginatedList<GetWantApprovalDto>>>
    {
        public string OrderId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    /// <summary>
    /// İstedim Onaylar Listesi
    /// </summary>
    public class GetWantApprovalQueryHandler : IRequestHandler<GetWantApprovalQuery, Response<PaginatedList<GetWantApprovalDto>>>
    {
        private IApplicationDbContext _context;

        public GetWantApprovalQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<PaginatedList<GetWantApprovalDto>>> Handle(GetWantApprovalQuery request, CancellationToken cancellationToken)
        {
            var list = await _context.Orders.Include(x=>x.Documents).Where(x => x.OrderId == request.OrderId).Select(x => new GetWantApprovalDto { OrderId = x.OrderId, Title = x.Title, IsDocument = x.Documents.Any(),NameAndSurname="",ProcessNo=x.Reference.ProcessNo,State=x.State,Date=x.Created.ToString("dd.MM.yyyy") }).OrderBy(x => x.Title).PaginatedListAsync(request.PageNumber, request.PageSize);
            return Response<PaginatedList<GetWantApprovalDto>>.Success(list, 200);
        }
    }
}
