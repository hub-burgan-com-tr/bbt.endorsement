using Application.Common.Models;
using MediatR;

namespace Application.Endorsements.Queries.GetMyApprovalsDetails
{
    public class GetMyApprovalDetailsQuery : IRequest<Response<List<GetMyApprovalDetailsDto>>>
    {/// <summary>
    /// Onay Id
    /// </summary>
        public int ApprovalId { get; set; }

    }
    /// <summary>
    /// Onayladıklarım Detay Sayfası
    /// </summary>
    public class GetMyApprovalDetailQueryHandler : IRequestHandler<GetMyApprovalDetailsQuery, Response<List<GetMyApprovalDetailsDto>>>
    {
        public async Task<Response<List<GetMyApprovalDetailsDto>>> Handle(GetMyApprovalDetailsQuery request, CancellationToken cancellationToken)
        {
            var list = new List<GetMyApprovalDetailsDto>();
            return Response<List<GetMyApprovalDetailsDto>>.Success(list, 200);
        }
    }
}
