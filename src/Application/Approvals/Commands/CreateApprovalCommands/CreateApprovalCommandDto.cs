using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Documents.Commands.CreateDocumentCommands;

namespace Application.Approvals.Commands.CreateApprovalCommands
{
    public class CreateApprovalCommandDto
    {
        /// <summary>
        /// InstanceId
        /// </summary>
        public string InstanceId { get; set; }
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
        /// Onaylar
        /// </summary>
        public string Approver { get; set; }
      
    }

   
}
