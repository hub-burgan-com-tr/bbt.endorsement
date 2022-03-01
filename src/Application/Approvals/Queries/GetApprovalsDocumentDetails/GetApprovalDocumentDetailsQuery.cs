using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Models;
using MediatR;

namespace Application.Approvals.Queries.GetApprovalsDocumentDetails
{
    public class GetApprovalDocumentDetailsQuery : IRequest<Response<GetApprovalDocumentDetailsDto>>
    {
        /// <summary>
        /// Onay Id
        /// </summary>
        public int ApprovalId { get; set; }
    }
    public class GetApprovalDocumentDetailsQueryHandler : IRequestHandler<GetApprovalDocumentDetailsQuery, Response<GetApprovalDocumentDetailsDto>>
    {
        public async Task<Response<GetApprovalDocumentDetailsDto>> Handle(GetApprovalDocumentDetailsQuery request, CancellationToken cancellationToken)
        {
            var result = new GetApprovalDocumentDetailsDto();
            return Response<GetApprovalDocumentDetailsDto>.Success(result, 200);
        }
    }
}
