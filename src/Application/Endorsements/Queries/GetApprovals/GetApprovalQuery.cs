using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Endorsements.Queries.GetApprovals
{
    public class GetApprovalQuery : IRequest<Response<PaginatedList<GetApprovalDto>>>
    {
        /// <summary>
        /// Instance Id
        /// </summary>
        public long CitizenshipNumber { get; set; }
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
            try
            {
                var list = await _context.Orders.Include(x => x.Documents)
                    .Where(x => x.Customer.CitizenshipNumber == request.CitizenshipNumber && x.State == OrderState.Pending.ToString())
                    
                    .OrderByDescending(x => x.Created)
                    .Select(x => new GetApprovalDto
                    {
                        OrderId = x.OrderId,
                        Title = x.Title,
                        IsDocument = x.Documents.Any(x => x.Type != ContentType.PlainText.ToString() && x.FormDefinitionId == null)
                    })
                    .PaginatedListAsync(request.PageNumber, request.PageSize);
                return Response<PaginatedList<GetApprovalDto>>.Success(list, 200);

            }
            catch (Exception ex)
            {
            }
            return Response<PaginatedList<GetApprovalDto>>.Success(200);
        }
    }
}
