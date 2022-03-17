using System.ComponentModel.DataAnnotations;

namespace Worker.App.Domain.Entities
{
    public class Document
    {
        [Key]
        [MaxLength(36)]
        public string DocumentId { get; set; }
        [MaxLength(250)]
        public string Name { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public virtual ICollection<Action> Actions { get; set; }

        [MaxLength(36)]
        public string OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
