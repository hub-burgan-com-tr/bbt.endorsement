using Application.Common.Models;
using MediatR;

namespace Application.Endorsements.Queries.GetApprovalsPhysicallyDocumentDetails
{
    public class GetApprovalPhysicallyDocumentDetailsQuery : IRequest<Response<List<GetApprovalPhysicallyDocumentDetailsDto>>>
    {
        /// <summary>
        /// Onay Id
        /// </summary>
        public int ApprovalId { get; set; }
    }
    public class GetApprovalDocumentDetailsQueryHandler : IRequestHandler<GetApprovalPhysicallyDocumentDetailsQuery, Response<List<GetApprovalPhysicallyDocumentDetailsDto>>>
    {
        public async Task<Response<List<GetApprovalPhysicallyDocumentDetailsDto>>> Handle(GetApprovalPhysicallyDocumentDetailsQuery request, CancellationToken cancellationToken)
        {
            var list = new List<GetApprovalPhysicallyDocumentDetailsDto>();
            return Response<List<GetApprovalPhysicallyDocumentDetailsDto>>.Success(list, 200);
        }
    }
}
