using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Worker.App.Domain.Common;

namespace Worker.App.Domain.Entities
{
    [Table("Approver", Schema = "approval")]
    public class Approver : AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string ApproverId { get; set; }
        [MaxLength(250)]
        public string FirstName { get; set; }
        [MaxLength(250)]
        public string LastName { get; set; }
        [MaxLength(11)]
        public long CitizenshipNumber { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
