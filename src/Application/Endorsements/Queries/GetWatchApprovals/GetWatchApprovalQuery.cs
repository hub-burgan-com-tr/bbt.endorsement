using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Mappings;
namespace Application.Endorsements.Queries.GetWatchApprovals
{
    public class GetWatchApprovalQuery : IRequest<Response<PaginatedList<GetWatchApprovalDto>>>
    {
    /// <summary>
    /// Onaylayan
    /// </summary>
        public string Approver { get; set; }
    /// <summary>
    /// Onay Isteyen
    /// </summary>
        public string Approval { get; set; }
        public string Process { get; set; }
        public string State { get; set; }
        public string ProcessNo { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }

    /// <summary>
    /// İzleme Listesi
    /// </summary>
    public class GetWatchApprovalQueryHandler : IRequestHandler<GetWatchApprovalQuery, Response<PaginatedList<GetWatchApprovalDto>>>
    {
        private IApplicationDbContext _context;
        public GetWatchApprovalQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<PaginatedList<GetWatchApprovalDto>>> Handle(GetWatchApprovalQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Order> orders = _context.Orders.OrderByDescending(x=>x.Created).Include(x=>x.Documents);
            if (!string.IsNullOrEmpty(request.Process))
                orders.Where(x => x.Reference.Process.Contains(request.Process));
            if (!string.IsNullOrEmpty(request.State))
                orders.Where(x => x.Reference.State.Contains(request.State));
            if (!string.IsNullOrEmpty(request.ProcessNo))
                orders.Where(x => x.Reference.ProcessNo==request.ProcessNo);
            var list = await orders
              .Select(x => new GetWatchApprovalDto
              {
                  OrderId = x.OrderId,
                  Title = x.Title,
                  Approval = "",
                  Approver = "",
                  Process = x.Reference.Process,
                  State = x.State,
                  ProcessNo = x.Reference.ProcessNo,
                  Date = x.Created.ToString("dd MM yyyy HH:mm"),
                  IsDocument = x.Documents.Any(y => y.Type == "PDF"),
                  OrderState = x.State,
              }).PaginatedListAsync(request.PageNumber,request.PageSize);

            return Response<PaginatedList<GetWatchApprovalDto>>.Success(list, 200);
        }
    }
}