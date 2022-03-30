using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("DocumentHistory", Schema = "order")]

    public class DocumentHistory : AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string DocumentHistoryId { get; set; }
        [MaxLength(36)]
        [Required]
        public string OrderId { get; set; }
        [MaxLength(36)]
        [Required]
        public string DocumentId { get; set; }
        [Required]
        [MaxLength(250)]
        public string State { get; set; }
        [MaxLength(250)]
        [Required]
        public string Name { get; set; }
        public virtual Document Document { get; set; }
        public virtual Order Order { get; set; }
    }
}
