using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Reference", Schema = "order")]
    public class Reference : AuditableEntity
    {
        public Reference()
        {
            Callbacks = new HashSet<Callback>();
        }


        [Key]
        [MaxLength(36)]
        public string OrderId { get; set; }
        /// <summary>
        /// Süreç
        /// </summary>
        [Required]
        [MaxLength(250)]
        public string Process { get; set; }

        /// <summary>
        /// Aşama
        /// </summary>
        [Required]
        [MaxLength(250)]
        public string State { get; set; }

        /// <summary>
        /// İşlem No
        /// </summary>
        [MaxLength(250)]
        public string ProcessNo { get; set; }

        public virtual Order Order { get; set; }
        public virtual ICollection<Callback> Callbacks { get; set; }
    }
}
