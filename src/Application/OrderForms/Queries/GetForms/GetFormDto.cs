using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderForms.Queries.GetForms
{
    public class GetFormDto
    {
        /// <summary>
        /// Form Id
        /// </summary>
        public string FormDefinitionId { get; set; }
        /// <summary>
        /// Form Ad
        /// </summary>
        public string Name { get; set; }
    }
}
