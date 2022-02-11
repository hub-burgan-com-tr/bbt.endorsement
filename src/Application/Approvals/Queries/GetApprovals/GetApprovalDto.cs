using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Approvals.Queries.GetApprovals
{
    public class GetApprovalDto : IMapFrom<Approval>
    {
        public int ApprovalId { get; set; }
        public string ApprovalName { get; set; }
        public bool IsDocument { get; set; }
    }
}
