using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Models;
using MediatR;

namespace Application.Approvals.Queries.GetApprovalCommandsDetails
{
    public class GetApprovalCommandDetailsQuery : IRequest<Response<GetApprovalCommandDetailsDto>>
    {
        public int ApprovalId { get; set; }
    }

    public class GetApprovalQueryHandler : IRequestHandler<GetApprovalCommandDetailsQuery, Response<GetApprovalCommandDetailsDto>>
    {
        public async Task<Response<GetApprovalCommandDetailsDto>> Handle(GetApprovalCommandDetailsQuery request, CancellationToken cancellationToken)
        {
            var result = new GetApprovalCommandDetailsDto();
            return Response<GetApprovalCommandDetailsDto>.Success(result, 200);
        }
    }
}
