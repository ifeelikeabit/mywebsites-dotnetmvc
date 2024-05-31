using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using whoindex.Data;
using whoindex.Models;

namespace whoindex.Controllers
{
    public class CourseController : Controller
    {
        private readonly DataContext _context;

        public CourseController(DataContext context)
        {
            _context = context;

        }


        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses.Include(s => s.Enrolments).ThenInclude(sc => sc.Students).ToListAsync();
            return View(courses);

        }
        public async Task<IActionResult> ManageStudents(int id)
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

        [HttpPost]
        public async Task<IActionResult> Enroll(int courseId, int studentId)
        {
            var studentCourse = new Enrolment
            {
                StudentId = studentId,
                CourseId = courseId
            };

            _context.Enrolments.Add(studentCourse);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageStudents), new { id = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> Unenroll(int courseId, int studentId)
        {
            var studentCourse = await _context.Enrolments
                .FirstOrDefaultAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);

            if (studentCourse != null)
            {
                _context.Enrolments.Remove(studentCourse);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ManageStudents), new { id = courseId });
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Course model)
        {
            if (_context.Students.Any(s => s.StudentId == model.CourseId))
            {

                ViewData["SearchedId"] = "fill";
                return View("Create");
            }
            _context.Courses.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
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


        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) { return NotFound(); }//id yoksa 404 sayfasına yönlendirir
            var coursemodel = await _context.Courses.FindAsync(id);
            if (coursemodel == null) { return NotFound(); }//hata olmasına karşın modelde sorgulanır 404 sayfasına yönlendirir
            _context.Courses.Remove(coursemodel);
            await _context.SaveChangesAsync();//Değişiklikleri kaydeder
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)//TODO: bu bilgi edit tuşuna basıldığında gelir(when? asp-action=="Edit")
        {

            if (id == null) { return NotFound(); }


            //* Eğer o değe*r yoksa null değer atanır
            //** FirstOrDefaultAsync sayesinde id yerine başka bir bilgi için aramada yapılabilir

            // var usr = await _context.Student.FindAsync(id);
            var usr = await _context.Courses.FirstOrDefaultAsync(usr => usr.CourseId == id);
            if (usr == null) { return NotFound(); }


            return View(usr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Course model)
        {
            if (id != model.CourseId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Courses.Any(o => o.CourseId == model.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }

                }
                return RedirectToAction("Index","Course");
            }
            return View(model);
        }
    }

}
