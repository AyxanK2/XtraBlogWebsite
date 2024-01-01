using System.ComponentModel.DataAnnotations;

namespace XtraBlogWebsite.Models
{
    public class Tag :BaseEntity
    {
        [StringLength(50),MinLength(3)]
        public string? Name { get; set; }
        [StringLength(70), MinLength(3)]
        public string? Slug{ get; set; }
    }
}
