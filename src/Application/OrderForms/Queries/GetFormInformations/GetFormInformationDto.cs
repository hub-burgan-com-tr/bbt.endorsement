using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderForms.Queries.GetFormInformations
{
    public class GetFormInformationDto
    {
        public string TemplateName { get; set; }
        public string Type { get; set; }
        public int MaxRetryCount { get; set; }
        public int RetryFrequence { get; set; }
        public int ExpireInMinutes { get; set; }
    }
}
