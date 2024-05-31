using whoindex.Data;

namespace whoindex.Models
{
    public class CourseStudentsViewModel
    {
        public Course Course { get; set; }
        public List<Student> Teachers { get; set; }
        public List<Student> Students { get; set; }
        public List<Student> EnrolledStudents { get; set; }
        public List<Student> EnrolledTeachers { get; set; }
    }

}