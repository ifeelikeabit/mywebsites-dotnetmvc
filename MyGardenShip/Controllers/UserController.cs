using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyGardenShip.Data;
using MyGardenShip.Models;

namespace MyGardenShip.Controllers
{
    public class UserController : Controller
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private IEmailSender _emailSender;
        private readonly IdentityContext _context;
        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailSender emailSender, IdentityContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _context = context;

        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            // var users = await _userManager.Users.ToListAsync();
            // foreach (var usr in users)
            // {
            //     await _userManager.DeleteAsync(usr);
            // }
            return View(user);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {

                    await _signInManager.SignOutAsync();

                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError("", "Hesabınızı onaylayın");
                        return View(model);
                    }

                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);
                        await _userManager.SetLockoutEndDateAsync(user, null);
                        return RedirectToAction("Index");
                    }
                    else if (result.IsLockedOut)
                    {
                        var LockoutDate = await _userManager.GetLockoutEndDateAsync(user);
                        var timeleft = LockoutDate.Value - DateTime.UtcNow;
                        ModelState.AddModelError("", $"Hesabınız kilitlendi, Lütfen {timeleft.Minutes} dakika bekleyin");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Parolanız hatalı");
                    }

                }
                else { ModelState.AddModelError("", "Hesap bulunamadı"); }
            }
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    FullName = model.UserName,
                    UserName = model.UserName,
                    Email = model.Email,
                };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var url = Url.Action("ConfirmEmail", "User", new { user.Id, token });
                    await _emailSender.SendEmailAsync(user.Email, "Hesap Onayı", $"<div style='display:flex;flex-direction:column;width:100%;height:100px;'> Hesabınızı onaylamak için lütfen aşağıdaki butona tıklayın.</br><a href='http://localhost:5059{url}' style='width:110px;height:20px;text-decoration:none;color:white; background-color:green; padding:10px;border-radius:10px;'>Hesabı onayla</a></div>");
                    TempData["message"] = "E-Mail hesabınızı onaylayın.";
                    return RedirectToAction("Login");
                }
                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                }
            }
            return View();
        }
        public async Task<IActionResult> ConfirmEmail(string Id, string token)
        {
            if (Id == null || token == null)
            {
                TempData["message"] = "Geçersiz token bilgisi";
                return View();
            }


            var user = await _userManager.FindByIdAsync(Id);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    TempData["message"] = "Hesabınız onaylandı";
                    return RedirectToAction("Login", "User");
                }
            }
            TempData["message"] = "Kullanıcı bulunamadı";
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }


        [HttpGet]
        public async Task<IActionResult> Delete()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _signInManager.SignOutAsync();
            var products = await _context.Products.ToListAsync();
            foreach (var p in products)
            {
                if (p.Email == user.Email)
                {
                    _context.Products.Remove(p);
                }
            }
            await _context.SaveChangesAsync();
            await _userManager.DeleteAsync(user);
            return RedirectToAction("Login");
        }
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, string UserName)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.UserName = UserName;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
    }
}