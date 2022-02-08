using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Models;
using MediatR;

namespace Application.Approvals.Queries.GetMyApprovals
{
    public class GetMyApprovalDetailQuery : IRequest<Response<List<GetMyApprovalDetailDto>>>
    {
        public int ApprovalId { get; set; }

    }
    public class GetMyApprovalDetailQueryHandler : IRequestHandler<GetMyApprovalDetailQuery, Response<List<GetMyApprovalDetailDto>>>
    {
        public async Task<Response<List<GetMyApprovalDetailDto>>> Handle(GetMyApprovalDetailQuery request, CancellationToken cancellationToken)
        {
            var list = new List<GetMyApprovalDetailDto>();
            return Response<List<GetMyApprovalDetailDto>>.Success(list, 200);
        }
    }
}
