using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using bordro.Models;
using bordro.Data;
using Microsoft.AspNetCore.Identity;
using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.CompilerServices;
namespace bordro.Controllers;
[Authorize]
public class CompanyController : Controller
{

    private readonly IdentityContext _context;
    private readonly UserManager<AppUser> _usermanager;
    private SignInManager<AppUser> _signinmanager;

    private RoleManager<AppRole> _rolemanager;
    public CompanyController(IdentityContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
    {
        _context = context;
        _usermanager = userManager;
        _signinmanager = signInManager;
        _rolemanager = roleManager;
    }
    public IActionResult Index()
    {
        var user = _context.Users.ToList();
        return View(user);
    }
    public IActionResult Register()
    {
        return View();
    }

 [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new AppUser
            {
                UserName = model.Username,
                Email = model.Username,
                Since = DateOnly.FromDateTime(DateTime.Now)
            };
            IdentityResult result = await _usermanager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _usermanager.AddToRoleAsync(user, "Company");
                return RedirectToAction("Index");
            }
            foreach (IdentityError err in result.Errors)
            {
                ModelState.AddModelError(string.Empty, err.Description);
            }
        }
        return View();
    }
 [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null) { return NotFound(); }
        var user = await _usermanager.FindByIdAsync(id);
        if (User.Identity.Name != "Admin") {
            await _signinmanager.SignOutAsync(); }


        await _usermanager.DeleteAsync(user);
     
        return RedirectToAction("Index", "Company");
    }
   
    public async Task<IActionResult> Edit(string? id,string? name)
    {
       AppUser user;
       
        if (id == null&&name==null)
        {
            return RedirectToAction("Index");

        }
        if(id!=null&&User.IsInRole("admin")){
          user= await _usermanager.FindByIdAsync(id);
        }else if(name!=null&&User.Identity.Name==name){
            user = await _usermanager.FindByNameAsync(name);
        }else  return RedirectToAction("Index");

       

        if (user != null)
        {
            return View(new EditViewModel
            {
                Id = user.Id,
                UserName = user.UserName
            });
        }
        return RedirectToAction("Index");

    }
    [HttpPost]
    public async Task<IActionResult> Edit(string id, EditViewModel model)
    {
        if (id != model.Id)
        {
            return RedirectToAction("Index");
        }
        if (ModelState.IsValid)
        {
            var user = await _usermanager.FindByIdAsync(model.Id);
            if (user != null)
            {
                user.UserName = model.UserName;

                var result = await _usermanager.UpdateAsync(user);

                if (result.Succeeded && !string.IsNullOrEmpty(model.Password))
                {
                    await _usermanager.RemovePasswordAsync(user);
                    await _usermanager.AddPasswordAsync(user, model.Password);
                }

                if (result.Succeeded)
                {

                    return RedirectToAction("Index");
                }
                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
        }
        return View(model);
    }


}
