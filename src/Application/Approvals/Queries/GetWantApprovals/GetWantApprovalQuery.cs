using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Models;
using MediatR;

namespace Application.Approvals.Queries.GetWantApprovals
{
    public class GetWantApprovalQuery : IRequest<Response<List<GetWantApprovalDto>>>
    {
        public string InstanceId { get; set; }

    }

    public class GetWantApprovalQueryHandler : IRequestHandler<GetWantApprovalQuery, Response<List<GetWantApprovalDto>>>
    {
        public async Task<Response<List<GetWantApprovalDto>>> Handle(GetWantApprovalQuery request, CancellationToken cancellationToken)
        {
            var list = new List<GetWantApprovalDto>();
            return Response<List<GetWantApprovalDto>>.Success(list, 200);
        }
    }
}
