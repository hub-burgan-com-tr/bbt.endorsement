using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Reference : AuditableEntity
    {
        public Reference()
        {
            Callbacks = new HashSet<Callback>();
        }

        [Key]
        public string ApprovalId { get; set; } = null!;
        public string InstanceId { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Number { get; set; } = null!;

        public virtual Approval Approval { get; set; } = null!;
        public virtual ICollection<Callback> Callbacks { get; set; }
    }
}
