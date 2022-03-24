using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// customer müşteri TCKN veya VKN bilgisini içerir. Kullanıcı kendi adına veya temsil ettiği organizasyon adına sözleşme onayı veriyor olabilir.
    /// </summary>
    [Table("Customer", Schema = "approval")]
    public class Customer : AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CitizenshipNumber { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
