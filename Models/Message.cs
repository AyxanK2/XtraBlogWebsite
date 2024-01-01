using System.ComponentModel.DataAnnotations;

namespace XtraBlogWebsite.Models
{
    public class Message :BaseEntity
    {
        [Required,StringLength(50)]
        public string? Name { get; set; }
        [Required,StringLength(50),EmailAddress]
        public string? Email { get; set; }
        [Required,StringLength(100)]
        public string? Subject { get; set; }
        [Required,StringLength(1000)]
        public string? Text { get; set; }
        public bool Accepted { get; set; } = false;
    }
}
