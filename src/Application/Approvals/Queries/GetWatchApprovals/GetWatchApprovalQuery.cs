using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Models;
using MediatR;

namespace Application.Approvals.Queries.GetWatchApprovals
{
    public class GetWatchApprovalQuery : IRequest<Response<List<GetWatchApprovalDto>>>
    {
        public string InstanceId { get; set; }
        public string Approver { get; set; }
        public string SeekingApproval { get; set; }

    }

    public class GetWatchApprovalQueryHandler : IRequestHandler<GetWatchApprovalQuery, Response<List<GetWatchApprovalDto>>>
    {
        public async Task<Response<List<GetWatchApprovalDto>>> Handle(GetWatchApprovalQuery request, CancellationToken cancellationToken)
        {
            var list = new List<GetWatchApprovalDto>();
            return Response<List<GetWatchApprovalDto>>.Success(list, 200);
        }
    }
}
