﻿using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("DocumentAction", Schema = "order")]
    public class DocumentAction : AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string DocumentActionId { get; set; }
        [MaxLength(36)]
        public string DocumentId { get; set; }
        [MaxLength(250)]
        public string Title { get; set; }
        [MaxLength(50)]
        public string Type { get; set; }
        [MaxLength(50)]
        public string State { get; set; }
        public int Choice { get; set; }
        public bool IsSelected { get; set; }
        public virtual Document Document { get; set; }

    }
}