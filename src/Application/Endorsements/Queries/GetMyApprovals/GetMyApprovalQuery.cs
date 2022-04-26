using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Endorsements.Queries.GetMyApprovals
{
    public class GetMyApprovalQuery : IRequest<Response<PaginatedList<GetMyApprovalDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    /// <summary>
    /// Onayladıklarım Listesi
    /// </summary>
    public class GetMyApprovalQueryHandler : IRequestHandler<GetMyApprovalQuery, Response<PaginatedList<GetMyApprovalDto>>>
    {
        private readonly IApplicationDbContext _context;

        public GetMyApprovalQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
        }

        public async Task<Response<PaginatedList<GetMyApprovalDto>>> Handle(GetMyApprovalQuery request, CancellationToken cancellationToken)
        {
            var list = await _context.Orders.Where(x => x.State != OrderState.Pending.ToString()&&x.State!=OrderState.Cancel.ToString())
                .Include(x => x.Documents)
                .OrderByDescending(x => x.Created)
                .Select(x => new GetMyApprovalDto
                {
                    OrderId = x.OrderId,
                    Title = x.Title,
                    IsDocument = x.Documents.Any(x => x.Type != ContentType.PlainText.ToString() && x.FormDefinitionId == null),
                    State = x.State,
                })
                .PaginatedListAsync(request.PageNumber, request.PageSize);
            return Response<PaginatedList<GetMyApprovalDto>>.Success(list, 200);

        }
    }


}
