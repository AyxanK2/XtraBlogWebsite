using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace XtraBlogWebsite.Models
{
    public class Setting :BaseEntity
    {
        [EmailAddress,StringLength(80)]
        public string Email { get; set; }
        [StringLength(250)]
        public string Adress { get; set; }
        [StringLength (100)]
        public string? Text { get; set; }
        [DataType(DataType.PhoneNumber)]
        public int Phone { get; set; }
        [StringLength(100),MaybeNull]
        public string? Facebook { get; set; }
        [StringLength(100), MaybeNull]
        public string? Linkedin { get; set; }
        [StringLength(100), MaybeNull]
        public string? Instagram { get; set; }
        [StringLength(100), MaybeNull]
        public string? Twitter { get; set; }
    }
}
