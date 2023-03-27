using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SSOIntegrationService.Models.Request
{
    public class SearchUserInfo
    {
        public string UserName { get; set; }
        public string FirtName { get; set; }
        public string LastName { get; set; }
    }
}
