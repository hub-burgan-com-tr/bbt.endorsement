using Application.Common.Models;
using MediatR;

namespace Application.Endorsements.Queries.GetApprovalsFormDocumentDetail
{
    public class GetApprovalFormDocumentDetailQuery:IRequest<Response<List<GetApprovalFormDocumentDetailDto>>>
    {
        public int ApprovalId { get; set; }
        public class GetApprovalsFormDocumentDetailQueryHandler : IRequestHandler<GetApprovalFormDocumentDetailQuery, Response<List<GetApprovalFormDocumentDetailDto>>>
        {
            public async Task<Response<List<GetApprovalFormDocumentDetailDto>>> Handle(GetApprovalFormDocumentDetailQuery request, CancellationToken cancellationToken)
            {
                var list = new List<GetApprovalFormDocumentDetailDto>();
                return Response<List<GetApprovalFormDocumentDetailDto>>.Success(list, 200);
            }
        }
    }
}
