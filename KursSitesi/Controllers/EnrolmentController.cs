using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using whoindex.Data;
using whoindex.Models;

namespace whoindex.Controllers
{
    public class EnrolmentController : Controller
    {
        private readonly DataContext _context;

        public EnrolmentController(DataContext context)
        {
            _context = context;

        }
        // public async Task<IActionResult> Index()
        // {
        //     var students = await _context.Students.Include(s => s.Enrolments).ThenInclude(sc => sc.Courses).ToListAsync();
        //     return View(students);

        // }
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses.Include(s => s.Enrolments).ThenInclude(sc => sc.Students).ToListAsync();
            return View(courses);
            
        }
       

       


        public async Task<IActionResult> Courses(int studentId)
        {
            var student = await _context.Students
                .Include(s => s.Enrolments)
                .ThenInclude(sc => sc.Courses)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null)
            {
                return NotFound();
            }

            var courses = await _context.Courses.ToListAsync();
            var viewModel = new EnrollViewModel
            {
                Student = student,
                Courses = courses
            };

            return View(viewModel);
        }
        // public async Task<IActionResult> Create()
        // {
        //     ViewBag.Course = new SelectList(await _context.Courses.ToListAsync(), "CourseId", "CourseName");
        //     ViewBag.Student = new SelectList(await _context.Students.ToListAsync(), "StudentId", "FullName");
        //     return View();
        // }
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create(Enrolment model)
        // {
        //     if (model == null)
        //     {
        //         return NotFound();
        //     }

        //     if (!ModelState.IsValid)
        //     {
        //         ViewBag.Course = new SelectList(await _context.Courses.ToListAsync(), "CourseId", "CourseName");
        //         ViewBag.Student = new SelectList(await _context.Students.ToListAsync(), "StudentId", "FullName");
        //         return View(model);
        //     }

        //     model.EnrolmentDate = DateTime.Now;
        //     await _context.Enrolments.AddAsync(model);
        //     await _context.SaveChangesAsync();

        //     return RedirectToAction(nameof(Index));
        // }




    }
}