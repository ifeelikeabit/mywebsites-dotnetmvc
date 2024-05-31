using System.ComponentModel.DataAnnotations;

namespace whoindex.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }
    }
}
