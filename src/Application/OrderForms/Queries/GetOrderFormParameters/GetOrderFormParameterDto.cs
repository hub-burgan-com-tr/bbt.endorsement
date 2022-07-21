using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderForms.Queries.GetOrderFormParameters
{
    public class GetOrderFormParameterDto
    {
        public List<ParameterDto> Process { get; set; }
        public List<ParameterDto> States { get; set; }
        public List<OrderDefinionParameterDto> Titles { get; set; }


    }

    public class ParameterDto
    {
        public string Id { get; set; }
        public string Text { get; set; }
    }
    public class OrderDefinionParameterDto
    {
        public string OrderDefinionId { get; set; }
        public string Title { get; set; }
    }
}
