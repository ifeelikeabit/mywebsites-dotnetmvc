using Microsoft.AspNetCore.Identity;

namespace mowbank.Models
{
    public class AppUser : IdentityUser
    {

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
    }

}