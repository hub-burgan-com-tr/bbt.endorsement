using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worker.App.Domain.Common;

namespace Worker.App.Domain.Entities
{
    public class Action : AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string OrderId { get; set; }
        [MaxLength(250)]
        public string Title { get; set; }
        public string Type { get; set; }
        public bool IsDefault { get; set; }
        [MaxLength(36)]
        public string DocumentId { get; set; }
        public virtual Document Document { get; set; }

    }
}
