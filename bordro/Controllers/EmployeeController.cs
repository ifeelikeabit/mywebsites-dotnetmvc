using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using bordro.Models;
using bordro.Data;
using Microsoft.AspNetCore.Identity;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace bordro.Controllers;

public class EmployeeController : Controller
{

    private readonly IdentityContext _context;
    private readonly UserManager<AppUser> _usermanager;
    private SignInManager<AppUser> _signinmanager;
    public EmployeeController(IdentityContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _context = context;
        _usermanager = userManager;
        _signinmanager = signInManager;
    }
    public async Task<IActionResult> Index()
    {
        var _user = await _usermanager.GetUserAsync(User);
        if (_user == null)
        {
            return RedirectToAction("Login", "Account"); // Eğer kullanıcı bulunamazsa giriş sayfasına yönlendir
        }

        var employees = await _context.Employees
                                      .Where(e => e.CompanyName == _user.UserName)
                                      .ToListAsync();

        return View(employees);
    }
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(EmployeeViewModel model)
    {
        var user = await _usermanager.GetUserAsync(User);
        if (ModelState.IsValid)
        {
            // TaxNumber'ın benzersiz olup olmadığını kontrol et
            var existingEmployee = await _context.Employees
                                                 .FirstOrDefaultAsync(e => e.TaxNumber == model.TaxNumber);
            if (existingEmployee != null)
            {
                ModelState.AddModelError("TaxNumber", "Bu vergi numarası zaten kayıtlı.");
                return View(model);
            }

            var employee = new Employee
            {
                TaxNumber = model.TaxNumber,
                CompanyName = user.UserName,
                Name = model.Name,
                Surname = model.Surname,
                DailysSalary = model.DailysSalary,
                EntryDate = model.EntryDate,
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Employee");
        }
        return View(model);
    }



    [HttpPost]
    public async Task<IActionResult> Delete(long taxnumber)
    {
        if (taxnumber <= 0)
        {
            return NotFound();
        }

        var employee = await _context.Employees.FindAsync(taxnumber);
        if (employee == null)
        {
            return NotFound();
        }

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync(); // await ile asenkron işlemi tamamlıyoruz

        return RedirectToAction("Index", "Employee");
    }

    public async Task<IActionResult> Details(long id)
    {
        if (id <= 0)
        {
            return NotFound();
        }

        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        return View(employee);
    }
    public async Task<IActionResult> Bordro(long id)
    {
        var employee = await _context.Employees.FindAsync(id);
        return View(new Bordro { id = id, employee = employee });
    }
    [HttpPost]
    public async Task<IActionResult> Bordro(long id, int day, int kesintiler)
    {
        var employee = await _context.Employees.FindAsync(id);

        return View(new Bordro { day = day, bordro = (employee.DailysSalary * day) - kesintiler, brüt =(employee.DailysSalary * day),id = id, kesintiler = kesintiler , employee=employee , isCalculate =true});
    }
    public async Task<IActionResult> Edit(long id)
    {
        if (id <= 0)
        {
            return NotFound();
        }

        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        var model = new EmployeeViewModel
        {
            TaxNumber = employee.TaxNumber,
            Name = employee.Name,
            Surname = employee.Surname,
            DailysSalary = employee.DailysSalary,
            EntryDate = employee.EntryDate
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(long id, EmployeeViewModel model)
    {
        if (id != model.TaxNumber)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            employee.Name = model.Name;
            employee.Surname = model.Surname;
            employee.DailysSalary = model.DailysSalary;
            employee.EntryDate = model.EntryDate;

            _context.Update(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

}
