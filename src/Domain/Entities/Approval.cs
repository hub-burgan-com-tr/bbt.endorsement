using Domain.Common;

namespace Domain.Entities
{
    public class Approval : AuditableEntity, IHasDomainEvent
    {
        public string ApprovalId { get; set; } = null!;
        public string ApprovalTitle { get; set; } = null!;
        public string InstanceId { get; set; } = null!;

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
