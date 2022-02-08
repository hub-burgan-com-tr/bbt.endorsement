using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Approvals.Queries.GetApprovals
{
    public class GetApprovalDetailDto
    {
        public string ApprovalName { get; set; }
        public string ApprovalDescription { get; set; }
        public string DocumentLink { get; set; }
        public bool IsDocumentApproved { get; set; }

    }
}
