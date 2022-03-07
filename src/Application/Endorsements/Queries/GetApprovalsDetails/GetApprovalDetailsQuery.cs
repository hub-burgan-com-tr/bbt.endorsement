using Application.Common.Models;
using MediatR;

namespace Application.Endorsements.Queries.GetApprovalsDetails
{
    public class GetApprovalDetailsQuery : IRequest<Response<List<GetApprovalDetailsDto>>>
    {
        /// <summary>
        /// Onay Id
        /// </summary>
        public int ApprovalId { get; set; }
    }
    /// <summary>
    /// Onayimdakiler Detay Sayfasi
    /// </summary>
    public class GetApprovalDetailQueryHandler : IRequestHandler<GetApprovalDetailsQuery, Response<List<GetApprovalDetailsDto>>>
    {
        public async Task<Response<List<GetApprovalDetailsDto>>> Handle(GetApprovalDetailsQuery request, CancellationToken cancellationToken)
        {
            var list = new List<GetApprovalDetailsDto>();
            return Response<List<GetApprovalDetailsDto>>.Success(list, 200);
        }
    }
}
