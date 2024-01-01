using System.ComponentModel.DataAnnotations;

namespace XtraBlogWebsite.Models
{
    public class Category :BaseEntity
    {
        [StringLength(50),MinLength(3,ErrorMessage = "Category's name min length must be at least 3")]
        public string Name { get; set; }
        public string? Slug { get; set; }
    }
}
