using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Approvals.Queries.GetApprovals
{
    public class GetApprovalDto : IMapFrom<Approval>
    {
        /// <summary>
       /// Onay Id
      /// </summary>
        public int ApprovalId { get; set; }
        /// <summary>
        /// Onay Ad
        /// </summary>
        public string ApprovalName { get; set; }
        /// <summary>
        /// Belge Var Mı
        /// </summary>
        public bool IsDocument { get; set; }
    }
}
