using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using whoindex.Data;
using whoindex.Models;

namespace whoindex.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        public HomeController(DataContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> List()
        {
            var courses = await _context.Courses.Include(s => s.Enrolments).ThenInclude(sc => sc.Students).ToListAsync();
            return View(courses);

        }

        [HttpPost]
        public async Task<IActionResult> Search(int id)
        {

            var student = await _context.Students
                       .Include(s => s.Enrolments)
                           .ThenInclude(sc => sc.Courses)
                       .FirstOrDefaultAsync(m => m.StudentId == id);

            if (student == null)
            {
                ViewData["SearchedId"] = id; return View("Index");
            }

            return View(student);
        }
        public async Task<IActionResult> Details(int id)
        {
            var course = await _context.Courses.Include(s => s.Enrolments).ThenInclude(sc => sc.Students)
               .FirstOrDefaultAsync(m => m.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }
            var users = await _context.Students.ToListAsync();
            var students = users.FindAll(u => u.isStudent);
            var teachers = users.FindAll(u => u.isTeacher);
            var enrolled = course.Enrolments.Select(sc => sc.Students).ToList();
            var enrolledStudents = enrolled.FindAll(u => u.isStudent);
            var enrolledSTeachers = enrolled.FindAll(u => u.isTeacher);

            var viewModel = new CourseStudentsViewModel
            {
                Course = course,
                Students = students,
                Teachers = teachers,

                EnrolledStudents = enrolledStudents,
                EnrolledTeachers = enrolledSTeachers
            };

            return View(viewModel);
        }


    }
}
