using Application.Common.Models;
using MediatR;

namespace Application.Approvals.Queries.GetWatchApprovalsDetails
{
    public class GetWatchApprovalDetailsQuery : IRequest<Response<GetWatchApprovalDetailsDto>>
    {
    /// <summary>
    /// Onay Id
    /// </summary>
        public int ApprovalId { get; set; }
    }

    /// <summary>
    /// İzleme Detay Sayfasi
    /// </summary>
    public class GetWatchApprovalDetailsQueryHandler : IRequestHandler<GetWatchApprovalDetailsQuery, Response<GetWatchApprovalDetailsDto>>
    {
        public async Task<Response<GetWatchApprovalDetailsDto>> Handle(GetWatchApprovalDetailsQuery request, CancellationToken cancellationToken)
        {
            var result = new GetWatchApprovalDetailsDto();
            return Response<GetWatchApprovalDetailsDto>.Success(result, 200);
        }
    }
}
