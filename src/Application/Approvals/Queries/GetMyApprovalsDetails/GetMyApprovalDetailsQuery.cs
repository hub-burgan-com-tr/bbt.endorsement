using Application.Common.Models;
using MediatR;

namespace Application.Approvals.Queries.GetMyApprovalsDetails
{
    public class GetMyApprovalDetailsQuery : IRequest<Response<List<GetMyApprovalDetailsDto>>>
    {
        public int ApprovalId { get; set; }

    }
    public class GetMyApprovalDetailQueryHandler : IRequestHandler<GetMyApprovalDetailsQuery, Response<List<GetMyApprovalDetailsDto>>>
    {
        public async Task<Response<List<GetMyApprovalDetailsDto>>> Handle(GetMyApprovalDetailsQuery request, CancellationToken cancellationToken)
        {
            var list = new List<GetMyApprovalDetailsDto>();
            return Response<List<GetMyApprovalDetailsDto>>.Success(list, 200);
        }
    }
}
