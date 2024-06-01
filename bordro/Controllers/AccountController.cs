using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using bordro.Models;
using bordro.Data;
using Microsoft.AspNetCore.Identity;
namespace bordro.Controllers;

public class AccountController : Controller
{
    private readonly IdentityContext _context;
    private readonly UserManager<AppUser> _usermanager;
    private SignInManager<AppUser> _signinmanager;
    public AccountController(IdentityContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _context = context;
        _usermanager = userManager;
        _signinmanager = signInManager;
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
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new AppUser { UserName = model.Username, Email = model.Username, Since = DateOnly.FromDateTime(DateTime.Now) };
            IdentityResult result = await _usermanager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {

                return RedirectToAction("Login");

            }
            foreach (IdentityError err in result.Errors)
            {
                ModelState.AddModelError(string.Empty, err.Description);
            }
        }
        return View();
    }
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _usermanager.FindByNameAsync(model.Username);
            if (user != null)
            {
                await _signinmanager.SignOutAsync();

                var result = await _signinmanager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    await _usermanager.ResetAccessFailedCountAsync(user);
                    await _usermanager.SetLockoutEndDateAsync(user, null);
                    return RedirectToAction("Index");
                }
                else if (result.IsLockedOut)
                {
                    var LockoutDate = await _usermanager.GetLockoutEndDateAsync(user);
                    var timeleft = LockoutDate.Value - DateTime.UtcNow;
                    ModelState.AddModelError("", $"Hesabınız kilitlendi, Lütfen {timeleft.Minutes} dakika bekleyin");
                }
                else
                {
                    ModelState.AddModelError("", "Parolanız hatalı");
                }

            }
            else { ModelState.AddModelError("", "Hesap bulunamadı"); }



            return RedirectToAction("Index", "Company");
        }
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await _signinmanager.SignOutAsync();
        return RedirectToAction("Login");
    }


}
