using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyGardenShip.Data;
using MyGardenShip.Models;

namespace MyGardenShip.Controllers;

[Authorize(Roles = "admin")]
public class AdminController : Controller
{
    private UserManager<AppUser> _userManager;
    private readonly IdentityContext _context;
    public AdminController(UserManager<AppUser> userManager, IdentityContext context)
    {
        _userManager = userManager;
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.ToListAsync();
        var products = await _context.Products.ToListAsync();
        var model = new AdminPageViewModel
        {
            userCount = users.Count,
            productCount = products.Count
        };

        return View(model);
    }
    public async Task<IActionResult> Users()
    {
        var temp_users = await _userManager.Users.ToListAsync();
        var users = new List<AppUser>();
foreach (var user in temp_users)
{
  if(user.Email!="admin@gmail.com")
    users.Add(user);
}

        return View(users);
    }
    public async Task<IActionResult> Products()
    {
        var products = await _context.Products.ToListAsync();


        return View(products);
    }
    [HttpGet]
    public async Task<IActionResult> EditUser(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var user = await _userManager.FindByIdAsync(id);
        var model = new EditUserViewModel
        {
            Id = user.Id,
            Email = user.Email,
            EmailConfirmed = user.EmailConfirmed,
            UserName = user.UserName,
        };
        Console.WriteLine(model.Id);
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> EditUser(string id, string UserName, string Email, string EmailConfirmed)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (ModelState.IsValid)
        {
            if (UserName != user.UserName)
            {
                user.UserName = UserName;
            }
            if (Email != user.Email)
            {
                user.Email = Email;
            }
            if (EmailConfirmed == "1" && !user.EmailConfirmed)
            {
                user.EmailConfirmed = true;
            }
            else if (EmailConfirmed != "1" && user.EmailConfirmed)
            {
                user.EmailConfirmed = false;
            }
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Users", "Admin");
        }


        return View();
    }



}
