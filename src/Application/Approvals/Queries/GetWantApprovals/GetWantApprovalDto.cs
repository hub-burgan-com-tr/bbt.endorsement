using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Approvals.Queries.GetWantApprovals
{
    public class GetWantApprovalDto
    {
        public int ApprovalId { get; set; }
        public string ApprovalName { get; set; }
        public string Approver { get; set; }
        public string TransactionNumber { get; set; }
        public string ApproverDate { get; set; }
        public bool IsDocument { get; set; } 
        public string ApprovalIcon { get; set; }
        public string ApprovalState { get; set; }

    }
}
