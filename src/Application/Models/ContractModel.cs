using Application.Endorsements.Commands.NewOrders;

namespace Application.Models
{
    public class ContractModel
    {
        public StartRequest StartRequest { get; set; }
        public Guid InstanceId { get; set; }
        public bool Device { get; set; }

        public bool Approved { get; set; }
        public bool Completed { get; set; }
        public bool IsProcess { get; set; }
        public bool RetryEnd { get; set; }
        public int Limit { get; set; }

    }
}
