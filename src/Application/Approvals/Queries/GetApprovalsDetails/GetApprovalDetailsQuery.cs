using Application.Common.Models;
using MediatR;

namespace Application.Approvals.Queries.GetApprovalsDetails
{
    public class GetApprovalDetailsQuery : IRequest<Response<List<GetApprovalDetailsDto>>>
    {
        public int ApprovalId { get; set; }
    }

    public class GetApprovalDetailQueryHandler : IRequestHandler<GetApprovalDetailsQuery, Response<List<GetApprovalDetailsDto>>>
    {
        public async Task<Response<List<GetApprovalDetailsDto>>> Handle(GetApprovalDetailsQuery request, CancellationToken cancellationToken)
        {
            var list = new List<GetApprovalDetailsDto>();
            return Response<List<GetApprovalDetailsDto>>.Success(list, 200);
        }
    }
}
