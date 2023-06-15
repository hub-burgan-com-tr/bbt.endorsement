using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TsizlFora.Model
{
    public class TSIZLResponse
    {
        public string ReferenceNumber { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }

    }
}
