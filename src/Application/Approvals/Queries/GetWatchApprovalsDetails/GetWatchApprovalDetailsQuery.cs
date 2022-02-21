using Application.Common.Models;
using MediatR;

namespace Application.Approvals.Queries.GetWatchApprovalsDetails
{
    public class GetWatchApprovalDetailsQuery : IRequest<Response<List<GetWatchApprovalDetailsDto>>>
    {
    /// <summary>
    /// Onay Id
    /// </summary>
        public int ApprovalId { get; set; }
    }

    /// <summary>
    /// İzleme Detay Sayfasi
    /// </summary>
    public class GetWatchApprovalDetailsQueryHandler : IRequestHandler<GetWatchApprovalDetailsQuery, Response<List<GetWatchApprovalDetailsDto>>>
    {
        public async Task<Response<List<GetWatchApprovalDetailsDto>>> Handle(GetWatchApprovalDetailsQuery request, CancellationToken cancellationToken)
        {
            var list = new List<GetWatchApprovalDetailsDto>();
            return Response<List<GetWatchApprovalDetailsDto>>.Success(list, 200);
        }
    }
}
