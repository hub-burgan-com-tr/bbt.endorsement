using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderForms.Queries.GetFormApprovar
{
    public class GetFormApprovarDto
    {
        /// <summary>
        /// Form Ad
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Onaycı
        /// </summary>
        public string ApproverName { get; set; }
        /// <summary>
        /// Belge Ad
        /// </summary>
        public string DocumentName { get; set; }
        /// <summary>
        /// Belge Link
        /// </summary>
        public string DocumentLink { get; set; }
    }
}
