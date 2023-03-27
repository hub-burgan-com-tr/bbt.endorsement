using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SSOIntegrationService.Models
{
    public class UserInfo
    {
        public string Id { get; set; }
        public string CitizenshipNumber { get; set; }

        public string LoginName { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string BranchCode { get; set; }
        public string RegisterId { get; set; }

        public string CustomerNo { get; set; }

    }


}
