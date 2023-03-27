using Application.SSOIntegrationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SSOIntegrationService.Commands
{
    public class SSOIntegrationResponse
    {
        public SSOIntegrationResponse() { }
        public List<UserAuthority> UserAuthorities { get; set; }
        public UserInfo UserInfo { get; set; }
        public string RegisterId { get; set; }
    }
}
