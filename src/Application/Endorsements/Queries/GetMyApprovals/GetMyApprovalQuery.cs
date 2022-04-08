using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        private readonly IMapper _mapper;

        public GetMyApprovalQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<PaginatedList<GetMyApprovalDto>>> Handle(GetMyApprovalQuery request, CancellationToken cancellationToken)
        {
            var list = await _context.Orders
                .Include(x => x.Documents)
                .OrderByDescending(x => x.Created)
                .Select(x => new GetMyApprovalDto
                {
                    OrderId = x.OrderId,
                    Title = x.Title,
                    IsDocument = x.Documents.Any(x => x.Type == ContentType.File.ToString()),
                    State = x.State,
                })
                .PaginatedListAsync(request.PageNumber, request.PageSize);
            return Response<PaginatedList<GetMyApprovalDto>>.Success(list, 200);

        }
    }


}
