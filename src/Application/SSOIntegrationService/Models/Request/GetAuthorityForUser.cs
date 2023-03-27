using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SSOIntegrationService.Models.Request
{
    public class GetAuthorityForUser
    {
        public string authorityName { get; private set; } = "Credentials";
        public string applicationCode { get; private set; } = "MOBIL_ONAY";
    }
}
