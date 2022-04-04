using Worker.App.Domain.Enums;
using static Worker.App.Models.StartRequest;

namespace Worker.App.Models
{
    public class ContractModel
    {
        public ContractModel()
        {
            Documents = new List<ApproveOrderDocument>();
        }

        public StartRequest StartRequest { get; set; }
        public StartFormRequest StartFormRequest { get; set; }
        public Form FormType { get; set; }

        public Guid InstanceId { get; set; }
        public bool Device { get; set; }

        public bool Approved { get; set; }
        public bool Completed { get; set; }
        public bool IsProcess { get; set; }
        public bool RetryEnd { get; set; }
        public int Limit { get; set; }

        public int? MaxRetryCount { get; set; }
        public int RetryFrequence { get; set; }
        public int? ExpireInMinutes { get; set; }


        public ApproveOrderDocument Document { get; set; }
        public List<ApproveOrderDocument> Documents { get; set; }
    }

    public class ApproveOrderDocument
    {
        public string DocumentId { get; set; }
        public string ActionId { get; set; }
        public int Choice { get; set; }
        public bool IsSelected { get; set; }
    }
}
