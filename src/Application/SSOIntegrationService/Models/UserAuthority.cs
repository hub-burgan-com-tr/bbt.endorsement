using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SSOIntegrationService.Models
{
    public class UserAuthority
    {
        public string PropertyId { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string PropertyType { get; set; }
    }
}
