using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Document
    {
       
        public string DocumentId { get; set; }
        [MaxLength(250)]
        public string Name { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }

        public virtual Order Order { get; set; }
    }
}
