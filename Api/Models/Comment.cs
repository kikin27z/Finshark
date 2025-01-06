using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    [Table("Comments")]
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        [Column(TypeName = "timestamp without time zone")]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [Column(TypeName = "timestamp without time zone")]
        public DateTime? ModifiedOn { get; set; }

        public int? StockId { get; set; }
        public virtual Stock? Stock { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
