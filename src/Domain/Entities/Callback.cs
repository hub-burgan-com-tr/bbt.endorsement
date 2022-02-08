using Domain.Common;

namespace Domain.Entities
{
    public class Callback : AuditableEntity
    {
        public string CallbackId { get; set; } = null!;
        public string ReferenceId { get; set; } = null!;
        public string InstanceId { get; set; } = null!;

        public string Mode { get; set; } = null!;
        public string Url { get; set; } = null!;

        public virtual Reference Reference { get; set; } = null!;
    }
}
