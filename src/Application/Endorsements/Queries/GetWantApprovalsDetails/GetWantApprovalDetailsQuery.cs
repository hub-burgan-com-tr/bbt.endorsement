using Application.Common.Models;
using MediatR;

namespace Application.Endorsements.Queries.GetWantApprovalsDetails
{
    public class GetWantApprovalDetailsQuery : IRequest<Response<GetWantApprovalDetailsDto>>
    {
        /// <summary>
        /// Onay Id
        /// </summary>
        public int ApprovalId { get; set; }
    }

    /// <summary>
    /// İstedigim Onaylar Detay Sayfası
    /// </summary>
    public class GetWantApprovalDetailQueryHandler : IRequestHandler<GetWantApprovalDetailsQuery, Response<GetWantApprovalDetailsDto>>
    {
        public async Task<Response<GetWantApprovalDetailsDto>> Handle(GetWantApprovalDetailsQuery request, CancellationToken cancellationToken)
        {
            var result = new GetWantApprovalDetailsDto();
            return Response<GetWantApprovalDetailsDto>.Success(result, 200);
        }
    }
}
