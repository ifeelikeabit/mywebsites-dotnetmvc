using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace whoindex.Data
{
    public class User
    {
     
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Enrolment> Enrolments { get; set; }

        public string Surname { get; set; } = null!;

        public string FullName
        {
            get
            {
                return this.Name + " " + this.Surname;
            }
        }
        public string? Email { get; set; }
        public string? Adress { get; set; }
        public string? phoneNumber { get; set; }
        public Boolean isStudent {get;set;}
        public Boolean isTeacher {get;set;}


    }

}



