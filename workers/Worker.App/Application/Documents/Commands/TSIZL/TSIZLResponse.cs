using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worker.App.Application.Documents.Commands.TSIZL;

public class TSIZLResponse
{
    public string ReferenceNumber { get; set; }
    public bool HasError { get; set; }
    public string ErrorMessage { get; set; }
    public override string ToString()
    {
        return "ReferenceNumber:" +ReferenceNumber + " HasError:"+ HasError + " ErrorMessage:"+ ErrorMessage;
    }
}
