using Application.Approvals.Commands.CreateApprovalCommands;

namespace Infrastructure.Dtos
{
    public class ContractApprovalData
    {
        public string InstanceId { get; set; }
        public string EventingConnectionId { get; set; }
        public int Limit { get; set; }
        public bool Device { get; set; }
        public bool Approved { get; set; }
        public long Customer { get; set; }

        public bool Completed { get; set; }
        public bool RetryEnd { get; set; }
        public bool IsProcess { get; set; }

        public CreateApprovalRequest Request { get; set; }
    }
}
