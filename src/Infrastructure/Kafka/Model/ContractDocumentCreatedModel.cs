using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Kafka.Model
{
    public class ContractDocumentCreatedModel
    {
        public ContractDocumentCreatedModelData Data { get; set; }
        public string DataContentType { get; set; }
        public Guid Id { get; set; }
        public string PubSubName { get; set; }
        public string Source { get; set; }
        public string SpecVersion { get; set; }
        public DateTime Time { get; set; }
        public string Topic { get; set; }
        public string TraceId { get; set; }
        public string TraceParent { get; set; }
        public string TraceState { get; set; }
        public string Type { get; set; }
    }
}