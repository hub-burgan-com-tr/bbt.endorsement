using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace Application.Endorsements.Commands.NewApprovelOrder
{
    public class NewApprovalOrderCommand
    {
        /// <summary>
        /// InstanceId
        /// </summary>
        public string InstanceId { get; set; }
        /// <summary>
        /// Başlık
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// İşlem
        /// </summary>
        public string Process { get; set; }
        /// <summary>
        /// Aşama
        /// </summary>
        public string Step { get; set; }
        /// <summary>
        /// işlem No
        /// </summary>

        public string ProcessNo { get; set; }
        /// <summary>
        /// Geçerlilik
        /// </summary>

        public string Validity { get; set; }
        /// <summary>
        /// hatırlatma Frekansı
        /// </summary>
        public string ReminderFrequency { get; set; }
        /// <summary>
        /// HatırlatmaSayısı
        /// </summary>
        public int ReminderCount { get; set; }

        public Document[] Document { get; set; }
        public Approver Approver { get; set; }


    }

    public class Document
    {
        
        public string DocumentType { get; set; }
        public IFormFile[] File { get; set; }
        public string Content { get; set; }
        public int FormId { get; set; }
        public string CitizenShipNumber { get; set; }
        public string NameSurname { get; set; }
    }

 
    public class Approver
    {
        public int Type { get; set; }
        public string Value { get; set; }
        public string NameSurname { get; set; }
    }

}
