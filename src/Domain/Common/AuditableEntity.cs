using System.ComponentModel.DataAnnotations;

namespace Domain.Common
{
    public abstract class AuditableEntity
    {
        public DateTime Created { get; set; }
        [MaxLength(36)]
        public string? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        [MaxLength(36)]
        public string? LastModifiedBy { get; set; }
    }
}
