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
        [MaxLength(250)]
        public string FirstName { get; set; }
        [MaxLength(250)]
        public string LastName { get; set; }
        [MaxLength(11)]
        public long CitizenshipNumber { get; set; }
        public int CustomerNumber { get; set; }
        [MaxLength(250)]
        public string BranchCode { get; set; }
        [MaxLength(250)]
        public string BusinessLine { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
