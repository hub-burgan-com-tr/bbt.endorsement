using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Approvals.Commands.CreateApprovalCommands
{
    public class CreateApprovalCommandDto
    {
        public CreateApprovalCommand CreateApprovalCommand { get; set; }
        public string Approver { get; set; }
    }
}
