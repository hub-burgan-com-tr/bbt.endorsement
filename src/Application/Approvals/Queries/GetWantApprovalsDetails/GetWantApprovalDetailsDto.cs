using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Approvals.Commands.CreateApprovalCommands;
using Application.Documents.Commands.CreateDocumentCommands;

namespace Application.Approvals.Queries.GetWantApprovalsDetails
{
    public class GetWantApprovalDetailsDto
    {

        /// <summary>
        ///Baslik
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// İşlem
        /// </summary>
        public string Process { get; set; }
        /// <summary>
        /// Aşama
        /// </summary>
        public string Stage { get; set; }
        /// <summary>
        /// İşlem No
        /// </summary>
        public string TransactionNumber { get; set; }
        /// <summary>
        /// Geçerlilik
        /// </summary>
        public string TimeoutMinutes { get; set; }
        /// <summary>
        /// Hatırlatma Frekansı
        /// </summary>
        public string RetryFrequence { get; set; }
        /// <summary>
        /// Hatırlatma Sayısı
        /// </summary>
        public int MaxRetryCount { get; set; }

        /// <summary>
        /// Onay Id
        /// </summary>
        public int ApprovalId { get; set; }
        /// <summary>
        /// Onaycı
        /// </summary>
        public string Approver { get; set; }
        /// <summary>
        /// Belgeler
        /// </summary>
        public List<GetWantApprovalDocumentDetailsDto> Documents { get; set; }
        /// <summary>
        /// Tarihce
        /// </summary>
        public List<GetWantApprovalDetailsHistoryDto> GetWantApprovalDetailsHistory { get; set; }
    }

    public class GetWantApprovalDocumentDetailsDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Belge Tipi
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
    public class GetWantApprovalDetailsHistoryDto
    {
        /// <summary>
        ///İşlem Ad 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Belge
        /// </summary>
        public string DocumentName { get; set; }
        /// <summary>
        /// İşlem Tarihi
        /// </summary>
        public string CreatedDate { get; set; }

    }



}
