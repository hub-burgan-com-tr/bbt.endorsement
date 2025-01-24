using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Kafka.Model
{
    public class ContractDocumentCreatedModelData
    {
        public DateTime ApprovalDate { get; set; }
        public string ApprovalStatus { get; set; }
        public string Channel { get; set; }
        public string ClientCode { get; set; }
        public string ContractCode { get; set; }
        public Guid ContractInstanceId { get; set; }
        public int CustomerNo { get; set; }
        public string DocumentCode { get; set; }
        public Guid DocumentContentId { get; set; }
        public Guid DocumentInstanceId { get; set; }
        public string DocumentVersion { get; set; }
        public bool IsRenderOnlineMainFlow { get; set; }
        public string Language { get; set; }
        public int TargetUserBusinessLine { get; set; }
        public string UserReference { get; set; }
    }
}