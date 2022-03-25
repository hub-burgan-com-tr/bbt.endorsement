using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Worker.App.Domain.Common;

namespace Worker.App.Domain.Entities
{
    [Table("Customer", Schema = "approval")]

    public class Customer: AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string CustomerId { get; set; }
        [MaxLength(250)]
        public string FirstName { get; set; }
        [MaxLength(250)]
        public string LastName { get; set; }
        [MaxLength(11)]
        public long CitizenshipNumber { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
