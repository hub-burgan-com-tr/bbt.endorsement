using Application.Common.Models;
using MediatR;

namespace Application.Approvals.Queries.GetApprovalsDetails
{
    public class GetApprovalDetailsQuery : IRequest<Response<GetApprovalDetailsDto>>
    {
        /// <summary>
        /// Onay Id
        /// </summary>
        public int ApprovalId { get; set; }
    }
    /// <summary>
    /// Onayimdakiler Detay Sayfasi
    /// </summary>
    public class GetApprovalDetailQueryHandler : IRequestHandler<GetApprovalDetailsQuery, Response<GetApprovalDetailsDto>>
    {
        public async Task<Response<GetApprovalDetailsDto>> Handle(GetApprovalDetailsQuery request, CancellationToken cancellationToken)
        {
            var result = new GetApprovalDetailsDto();
            return Response<GetApprovalDetailsDto>.Success(result, 200);
        }
    }
}
