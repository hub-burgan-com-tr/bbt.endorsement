using Application.Common.Models;
using MediatR;

namespace Application.Approvals.Queries.GetApprovals
{
    public class GetApprovalQuery : IRequest<Response<List<GetApprovalDto>>>
    {/// <summary>
    /// Instance Id
    /// </summary>
        public string InstanceId { get; set; }
    }

    /// <summary>
    /// Onayimdakiler Listesi
    /// </summary>
    public class GetApprovalQueryHandler : IRequestHandler<GetApprovalQuery, Response<List<GetApprovalDto>>>
    {
        public async Task<Response<List<GetApprovalDto>>> Handle(GetApprovalQuery request, CancellationToken cancellationToken)
        {
            var list = new List<GetApprovalDto>();
            return Response<List<GetApprovalDto>>.Success(list, 200);
        }
    }
}
