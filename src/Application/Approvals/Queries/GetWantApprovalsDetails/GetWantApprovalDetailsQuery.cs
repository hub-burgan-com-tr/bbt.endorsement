using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Models;
using MediatR;

namespace Application.Approvals.Queries.GetWantApprovalsDetails
{
    public class GetWantApprovalDetailsQuery : IRequest<Response<List<GetWantApprovalDetailsDto>>>
    {
        /// <summary>
        /// Onay Id
        /// </summary>
        public int ApprovalId { get; set; }
    }

    /// <summary>
    /// İstedigim Onaylar Detay Sayfası
    /// </summary>
    public class GetWantApprovalDetailQueryHandler : IRequestHandler<GetWantApprovalDetailsQuery, Response<List<GetWantApprovalDetailsDto>>>
    {
        public async Task<Response<List<GetWantApprovalDetailsDto>>> Handle(GetWantApprovalDetailsQuery request, CancellationToken cancellationToken)
        {
            var list = new List<GetWantApprovalDetailsDto>();
            return Response<List<GetWantApprovalDetailsDto>>.Success(list, 200);
        }
    }
}
