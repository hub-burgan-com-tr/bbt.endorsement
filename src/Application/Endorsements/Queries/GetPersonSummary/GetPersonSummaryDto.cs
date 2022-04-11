using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Endorsements.Queries.GetPersonSummary
{
    public class GetPersonSummaryDto
    {
        public long CitizenshipNumber { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
    }
}
