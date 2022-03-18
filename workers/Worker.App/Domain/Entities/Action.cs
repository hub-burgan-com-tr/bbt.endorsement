using System.ComponentModel.DataAnnotations;
using Worker.App.Domain.Common;

namespace Worker.App.Domain.Entities
{
    public class Action : AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string ActionId { get; set; }
        [MaxLength(250)]
        public string Title { get; set; }
        public string Type { get; set; }
        public bool IsDefault { get; set; }
        [MaxLength(36)]
        public string DocumentId { get; set; }
        public virtual Document Document { get; set; }

    }
}
