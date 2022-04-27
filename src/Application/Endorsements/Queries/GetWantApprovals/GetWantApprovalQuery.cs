using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Endorsements.Queries.GetWantApprovals
{
    public class GetWantApprovalQuery : IRequest<Response<PaginatedList<GetWantApprovalDto>>>
    {
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
            var list = await _context.Orders.Include(x=>x.Documents).Include(x=>x.Customer).OrderByDescending(x => x.Created).Select(x => new GetWantApprovalDto { OrderId = x.OrderId, Title = x.Title, IsDocument = x.Documents.Any(x => x.Type != ContentType.PlainText.ToString() && x.FormDefinitionId == null),NameAndSurname=x.Customer.FirstName+" "+x.Customer.LastName+", "+x.Reference.ProcessNo,State=x.State,Date= x.Created.ToString("dd.MM.yyyy")
            }).PaginatedListAsync(request.PageNumber, request.PageSize);
            return Response<PaginatedList<GetWantApprovalDto>>.Success(list, 200);
        }
    }
}
