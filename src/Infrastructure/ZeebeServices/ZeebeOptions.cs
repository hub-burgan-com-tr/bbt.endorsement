using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ZeebeServices
{
    public class ZeebeOptions
    {
        public string ModelFilename { get; set; }
        public string ProcessPath { get; set; }
        public string EventHubUrl { get; set; }
    }
}
