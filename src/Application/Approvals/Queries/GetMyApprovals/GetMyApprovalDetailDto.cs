using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Approvals.Queries.GetMyApprovals
{
    public class GetMyApprovalDetailDto
    {
        public string ApprovalName { get; set; }
        public bool IsDocumentApproved { get; set; }
        public string ApprovalIcon { get; set; }
        public string Approver { get; set; }
        public string ApproverDate { get; set; }
    }
}
