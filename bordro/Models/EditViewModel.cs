using System.ComponentModel.DataAnnotations;

namespace bordro.Models
{



    public class EditViewModel
    {   
        public string Id { get; set; } 
        public string UserName { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string? Password { get; set; } = string.Empty;


        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Parola Eşleşmiyor")]
        public string? ConfirmPassword { get; set; }

        //Alttaki kısım kullanıcı ekstra bilgileri. Kullanıcı edit profile kısmında bu verileri güncellicek 
        public string? Name { get; set; }
        public string? Salary { get; set; }
    }
}