using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace XtraBlogWebsite.Models
{
    public class Post :BaseEntity
    {
        [StringLength(150)]
        public string? Title { get; set; }
        [StringLength(200)]
        public string? Slug { get; set; }
        public Category? Category { get; set; }
        public int CategoryId { get; set; }
        [StringLength(250)]
        public string? ShortDesc { get; set; }
        [StringLength(5000),MaybeNull]
        public string? LongDesc { get; set; }
        public DateTime Date { get; set; }
        [StringLength(100)]
        public string? Image { get; set; }
        public List<Comment>? Comment { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
        public List<TagPost>? TagPosts { get; set; }
        [NotMapped]
        public List<int>? TagIds { get; set; }
    }
}
