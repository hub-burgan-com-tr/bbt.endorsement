using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Models;
using MediatR;

namespace Application.Approvals.Queries.GetApprovals
{
    public class GetApprovalDetailQuery : IRequest<Response<List<GetApprovalDetailDto>>>
    {
        public int ApprovalId { get; set; }
    }

    public class GetApprovalDetailQueryHandler : IRequestHandler<GetApprovalDetailQuery, Response<List<GetApprovalDetailDto>>>
    {
        public async Task<Response<List<GetApprovalDetailDto>>> Handle(GetApprovalDetailQuery request, CancellationToken cancellationToken)
        {
            var list = new List<GetApprovalDetailDto>();
            return Response<List<GetApprovalDetailDto>>.Success(list, 200);
        }
    }
}
