using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using whoindex.Data;

namespace whoindex.Controllers
{
    public class StudentController : Controller
    {
        private readonly DataContext _context;

        public StudentController(DataContext context)
        {
            _context = context;

        }


        public async Task<IActionResult> Index()
        {
            var users = await _context.Students.ToListAsync();


            return View(users.FindAll(u => u.isStudent));
        }
        // public async Task<IActionResult> Detailed(int? id)
        // {

        //     if (id == null) { return NotFound(); }


        //     var usr = await _context.Student.FindAsync(id);
        //     if (usr == null) { return NotFound(); }


        //     return View(usr);//id ye ait kuulanıcı bilgilerini ilgili Detailed viewine model olarak gönderiyor
        // }
        public async Task<IActionResult> Details(int id)
        {
            var student = await _context.Students
                .Include(s => s.Enrolments)
                    .ThenInclude(sc => sc.Courses)
                .FirstOrDefaultAsync(m => m.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Student model)
        {
            if (_context.Students.Any(s => s.StudentId == model.StudentId))
            {
              
                 ViewData["SearchedId"] ="Bu id kullanılıyor"; 
                 return View("Create"); 
            }
            model.isStudent = true;
            _context.Students.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)//TODO: bu bilgi edit tuşuna basıldığında gelir(when? asp-action=="Edit")
        {

            if (id == null) { return NotFound(); }


            //* Eğer o değe*r yoksa null değer atanır
            //** FirstOrDefaultAsync sayesinde id yerine başka bir bilgi için aramada yapılabilir

            // var usr = await _context.Student.FindAsync(id);
            var usr = await _context.Students.FirstOrDefaultAsync(usr => usr.StudentId == id);
            if (usr == null) { return NotFound(); }


            return View(usr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student model)
        {
            if (id != model.StudentId)
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
                    if (!_context.Students.Any(o => o.StudentId == model.StudentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }

                }
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) { return NotFound(); }

            var usr = await _context.Students.FindAsync(id);

            if (usr == null) { return NotFound(); }


            return View(usr);
        }

        [HttpPost]

        public async Task<IActionResult> Delete(int id)
        {


            var usr = await _context.Students.FindAsync(id);
            if (usr == null) { return NotFound(); }
            _context.Students.Remove(usr);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }


    }




}