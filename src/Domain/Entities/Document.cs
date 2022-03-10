using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Document
    {
        public string DocumentId { get; set; }

        [Key]
        public string ApprovalId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }

        public virtual Order Approval { get; set; }
    }
}
