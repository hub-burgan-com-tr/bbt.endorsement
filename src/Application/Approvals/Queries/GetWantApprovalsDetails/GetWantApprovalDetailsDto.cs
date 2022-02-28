using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Approvals.Commands.CreateApprovalCommands;
using Application.Documents.Commands.CreateDocumentCommands;

namespace Application.Approvals.Queries.GetWantApprovalsDetails
{
    public class GetWantApprovalDetailsDto
    {
        /// <summary>
        /// Yeni Onay Emri Detay
        /// </summary>
        public CreateApprovalCommand ApprovalCommand { get; set; }
        /// <summary>
        /// Belgeler
        /// </summary>
        public List<CreateDocumentCommandDto> Documents { get; set; }
        /// <summary>
        /// Onay Id
        /// </summary>
        public int ApprovalId { get; set; }
        /// <summary>
        /// Onaylar
        /// </summary>
        public string Approver { get; set; }
    }
}
