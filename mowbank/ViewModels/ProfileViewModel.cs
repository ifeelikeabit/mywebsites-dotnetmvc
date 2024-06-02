using System.ComponentModel.DataAnnotations;

namespace mowbank.ViewModels
{



    public class ProfileViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? Full_Name { get; set; }
        public string? Adress { get; set; }
        public string? Job { get; set; }
        public int? age { get; set; }
        public string? gender { get; set; }
        public string? bio { get; set; }
        public string? hobby { get; set; }
        public string? nationality { get; set; }
        public decimal? salary { get; set; } // Nullable decimal tipi kullanıldı
        public string? marital_status { get; set; }
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [DataType(DataType.Password)]
        public string? Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Parola Eşleşmiyor")]
        public string? ConfirmPassword { get; set; }





    }
}