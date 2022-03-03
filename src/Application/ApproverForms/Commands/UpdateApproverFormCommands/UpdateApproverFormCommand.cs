using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApproverForms.Commands.UpdateApproverFormCommands
{
    public class UpdateApproverFormCommand
    {
        /// <summary>
        /// Form Id
        /// </summary>
        public int FormId { get; set; }
        /// <summary>
        /// Onay Id
        /// </summary>
        public int ApprovalId { get; set; }
        /// <summary>
        /// TCKN
        /// </summary>
        public string CitizenShipNumber { get; set; }
        /// <summary>
        /// Müsteri No
        /// </summary>
        public string CustomerNumber { get; set; }
    }
}
