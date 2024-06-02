using System.Diagnostics.CodeAnalysis;
using mowbank.Models;
using mowbank.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace mowbank
{

    public class AccountController : Controller
    {

        private UserManager<AppUser> _userManagar;

        private SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManagar, SignInManager<AppUser> signInManager)
        {
            _userManagar = userManagar;

            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManagar.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Detailed", "Profile");
                    }
                    else
                    {
                        ModelState.AddModelError("", "hatalı  parola");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "hatalı kullanıcı adı ya da parola");
                }
            }
            return View(model);

        }
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = new AppUser { UserName = model.UserName, Email = model.Email };

                IdentityResult result = await _userManagar.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {

            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult Bridge()
        {
            return View();
        }
    }
}
