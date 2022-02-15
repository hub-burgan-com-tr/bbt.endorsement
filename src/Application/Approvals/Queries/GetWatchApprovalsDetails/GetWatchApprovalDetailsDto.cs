using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Approvals.Commands.CreateApprovalCommands;
using Application.Documents.Commands.CreateDocumentCommands;

namespace Application.Approvals.Queries.GetWatchApprovalsDetails
{
    public class GetWatchApprovalDetailsDto
    {
        public CreateApprovalCommand ApprovalCommand { get; set; }
        public List<CreateDocumentCommandDto> Documents { get; set; }
        public int Id { get; set; }
        public string Approver { get; set; }
    }
}
