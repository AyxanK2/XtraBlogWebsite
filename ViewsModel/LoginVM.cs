using System.ComponentModel.DataAnnotations;

namespace XtraBlogWebsite.ViewsModel
{
    public class LoginVM
    {
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password),MinLength(8)]
        public string Password { get; set; }
    }
}
