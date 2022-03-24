using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// approvers belgelere onay verecek kullanıcıların TCKN numarasını içeririr
    /// </summary>
    [Table("Approver", Schema = "approval")]
    public class Approver : AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string ApproverId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CitizenshipNumber { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}