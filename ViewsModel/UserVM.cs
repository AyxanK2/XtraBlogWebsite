using System.ComponentModel.DataAnnotations;

namespace XtraBlogWebsite.ViewsModel
{
    public class UserVM
    {
        [EmailAddress]
        public string? Email { get; set; }
        public string? UserName { get; set; }
        [DataType(DataType.Password)]
        public string? OldPassword { get; set; }
        [DataType(DataType.Password),MinLength(8)]
        public string? NewPassword { get; set; }
        [DataType(DataType.Password),MinLength(8)]
        public string? ConfirmPassword { get; set; }

    }
}
