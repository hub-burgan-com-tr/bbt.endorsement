using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Documents.Commands.Queries.GetDocumentDetails
{
    public class GetDocumentDetailsDto
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
        /// Belge Tip Id 
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// Belge Tipi
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// Belge Adı
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Belge Onaylı Mı
        /// </summary>
        public bool IsDocumentApproved { get; set; }
        /// <summary>
        /// Belge Onay Ad
        /// </summary>
        public string DocumentApproved { get; set; }
        /// <summary>
        /// Baslik
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Metin
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// FormUd
        /// </summary>
        public int FormId { get; set; }
        /// <summary>
        /// TCKN
        /// </summary>
        public string CitizenShipNumber { get; set; }
        /// <summary>
        /// Ad Soyad
        /// </summary>
        public string NameAndSurname { get; set; }

        public List<GetDocumentDetailsApprovedDto> GetDocumentDetailsApproved { get; set; }
    }

    public class GetDocumentDetailsApprovedDto
    {
        public string ApprovedName { get; set; }
        public bool IsApproved { get; set; }
    }
}
