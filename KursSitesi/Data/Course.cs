using System.ComponentModel.DataAnnotations;
using whoindex.Data;

namespace whoindex
{

    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string? CourseName { get; set; }
        
        public ICollection<Enrolment> Enrolments { get; set; }





//Ekstralar

        public int quota { get; set; }
        public DateOnly CourseTime { get; set; }
        public TimeOnly start { get; set; }
        public TimeOnly end { get; set; }




    }






}