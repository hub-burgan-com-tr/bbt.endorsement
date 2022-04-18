using Application.OrderForms.Queries.GetProcess;
using Application.OrderForms.Queries.GetStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderForms.Queries.GetProcessAndState
{
    public class GetProcessAndStateDto
    {
        public List<GetProcessDto> Process { get; set; }
        public List<GetStateDto> State { get; set; }
    }
}
