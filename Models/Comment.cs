using System.ComponentModel.DataAnnotations;

namespace XtraBlogWebsite.Models
{
    public class Comment : BaseEntity
    {
        [EmailAddress,StringLength(100)]
        public string? Email { get; set; }
        [StringLength(100)]
        public string? Name { get; set; }
        [StringLength(1000)]
        public string? Text { get; set; }
        public int PostId { get; set; }
        public Post? Post { get; set; }
        public bool Accepted { get; set; } = false;
    }
}
