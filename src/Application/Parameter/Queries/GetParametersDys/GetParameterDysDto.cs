using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Parameter.Queries.GetParametersDys
{
    public class GetParameterDysDto
    {
        public string ParameterId { get; set; }
        public string Text { get; set; }
        public int? DmsReferenceId { get; set; }
        public int? DmsReferenceKey { get; set; }
        public string DmsReferenceName { get; set; }
    }
}
