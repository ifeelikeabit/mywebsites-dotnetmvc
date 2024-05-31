using System.ComponentModel.DataAnnotations;

namespace whoindex.Data
{

    public class Enrolment
    {

        public int StudentId { get; set; }
        public Student Students { get; set; }

        public int CourseId { get; set; }
        public Course Courses { get; set; }

        public DateTime EnrolmentDate { get; set; }

    }



}