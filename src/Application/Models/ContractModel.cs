using Application.Endorsements.Commands.NewOrders;
using Domain.Enum;

namespace Application.Models
{
    public class ContractModel
    {
        public StartRequest StartRequest { get; set; }
        public StartFormRequest StartFormRequest { get; set; }
        public Form FormType { get; set; }


        public Guid InstanceId { get; set; }
        public int MaxRetryCount { get; set; }
        public int RetryFrequence { get; set; }
        public int ExpireInMinutes { get; set; }


        public bool Device { get; set; }
        public bool Approved { get; set; }
        public bool Completed { get; set; }
        public bool IsProcess { get; set; }
        public bool RetryEnd { get; set; }
        public int Limit { get; set; }

        public object Document { get; set; }

    }
}