using Application.Common.Models;
using MediatR;

namespace Application.Endorsements.Queries.GetWantApprovals
{
    public class GetWantApprovalQuery : IRequest<Response<List<GetWantApprovalDto>>>
    {
        public string InstanceId { get; set; }

    }
    /// <summary>
    /// İstedim Onaylar Listesi
    /// </summary>
    public class GetWantApprovalQueryHandler : IRequestHandler<GetWantApprovalQuery, Response<List<GetWantApprovalDto>>>
    {
        public async Task<Response<List<GetWantApprovalDto>>> Handle(GetWantApprovalQuery request, CancellationToken cancellationToken)
        {
            var list = new List<GetWantApprovalDto>();
            return Response<List<GetWantApprovalDto>>.Success(list, 200);
        }
    }
}
