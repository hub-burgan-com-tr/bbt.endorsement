using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Documents.Commands.Queries.GetDocuments
{
    public class GetDocumentsDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Onay Id
        /// </summary>
        public int ApprovalId { get; set; }
        /// <summary>
        /// Belge Tip
        /// </summary>
        public string TypeId { get; set; }
        /// <summary>
        /// Belge Tipi Ad
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// Belge Adı
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Belge Onay Ad
        /// </summary>
        public string DocumentApproved { get; set; }
    }
}
