using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Models;
using MediatR;

namespace Application.Approvals.Queries.GetApprovalsDocumentList
{
    public class GetApprovalsDocumentListQuery:IRequest<Response<List<GetApprovalsDocumentListDto>>>
    {
        public int ApprovalId { get; set; }
    }

    public class GetApprovalsDocumentListQueryHandler : IRequestHandler<GetApprovalsDocumentListQuery, Response<List<GetApprovalsDocumentListDto>>>
    {
        public async Task<Response<List<GetApprovalsDocumentListDto>>> Handle(GetApprovalsDocumentListQuery request, CancellationToken cancellationToken)
        {
            var list = new List<GetApprovalsDocumentListDto>();
            return Response<List<GetApprovalsDocumentListDto>>.Success(list, 200);
        }
    }
}
