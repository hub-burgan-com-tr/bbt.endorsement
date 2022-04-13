using Worker.App.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Worker.App.Domain.Entities
{
    [Table("Config", Schema = "order")]

    public class Config : AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string OrderId { get; set; }
        /// <summary>
        ///  emrin geçerlilik süresi dakika olarak tanımlanır. 
        /// </summary>
        [Required]
        public int ExpireInMinutes { get; set; }


        /// <summary>
        /// hatırlatma frekansını belirlemek için kullanılır. 
        /// </summary>
        [Required]
        public int RetryFrequence { get; set; }

        /// <summary>
        /// kullanıcıya kaç defa hatırlatma yapılacağı bilgisini içerir. 
        /// </summary>
        [Required]
        public int MaxRetryCount { get; set; }


        public virtual Order Order { get; set; }
    }
}
