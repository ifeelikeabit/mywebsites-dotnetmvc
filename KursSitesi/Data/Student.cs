using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace whoindex.Data
{

    public class Student
    {
        public int StudentId { get; set; }


        public string? StudentName { get; set; }
        public ICollection<Enrolment>? Enrolments { get; set; }

        public string? StudentSurname { get; set; }

        public string FullName
        {
            get
            {
                return this.StudentName + " " + this.StudentSurname;
            }
        }
        public string? Email { get; set; }
        public string? Adress { get; set; }
        public string? phoneNumber { get; set; }


        public Boolean isStudent { get; set; }
        public Boolean isTeacher { get; set; }


    }

}



