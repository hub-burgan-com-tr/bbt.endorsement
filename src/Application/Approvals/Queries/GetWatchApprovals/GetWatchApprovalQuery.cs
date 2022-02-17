using Application.Common.Models;
using MediatR;

namespace Application.Approvals.Queries.GetWatchApprovals
{
    public class GetWatchApprovalQuery : IRequest<Response<List<GetWatchApprovalDto>>>
    {/// <summary>
    /// Instance Id
    /// </summary>
        public string InstanceId { get; set; }
    /// <summary>
    /// Onaylayan
    /// </summary>
        public string Approver { get; set; }
    /// <summary>
    /// Onay Isteyen
    /// </summary>
        public string SeekingApproval { get; set; }

    }

    /// <summary>
    /// İzleme Listesi
    /// </summary>
    public class GetWatchApprovalQueryHandler : IRequestHandler<GetWatchApprovalQuery, Response<List<GetWatchApprovalDto>>>
    {
        public async Task<Response<List<GetWatchApprovalDto>>> Handle(GetWatchApprovalQuery request, CancellationToken cancellationToken)
        {
            var list = new List<GetWatchApprovalDto>();
            return Response<List<GetWatchApprovalDto>>.Success(list, 200);
        }
    }
}