using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Kafka.Model
{
    public class ContractApprovedModelData
    {
        public List<ApprovedDocument> ApprovedDocuments { get; set; }
        public string Channel { get; set; }
        public string ContractCode { get; set; }
        public Guid ContractInstanceId { get; set; }
        public int CustomerNo { get; set; }
        public string Language { get; set; }
        public string Status { get; set; }
        public int TargetUserBusinessLine { get; set; }
        public string UserReference { get; set; }
    }

    public class ApprovedDocument
    {
        public DateTime ApprovalDate { get; set; }
        public string Code { get; set; }
        public string Version { get; set; }
    }
}