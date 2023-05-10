using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Parameter.Queries.GetParameters
{
    public class GetParameterDto
    {
        public string ParameterId { get; set; }
        public string Text { get; set; }
        public int? DmsReferenceId { get; set; }
        public int? DmsReferenceKey { get; set; }
        public string DmsReferenceName { get; set; }

    }
}
