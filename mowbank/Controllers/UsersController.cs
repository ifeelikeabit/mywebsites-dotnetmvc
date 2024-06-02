using mowbank.Models;
using mowbank.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;

namespace mowbank.Controllers
{
  
    public class UsersController : Controller
    {

        private UserManager<AppUser> _userManager;


        public UsersController(UserManager<AppUser> userManagar)
        {
            _userManager = userManagar;

        }
        [AllowAnonymous]
        public IActionResult Index(string searchString)
        {

            var users = from u in _userManager.Users
                        select u;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.UserName.Contains(searchString));
            }

            return View(users);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {

            if (User.Identity.Name != "admin") { return RedirectToAction("Bridge", "Account"); }

            return View();
        }
          [Authorize(Roles = "admin")]

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (User.Identity.Name != "admin") { return RedirectToAction("Bridge", "Account"); }
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.UserName, Email = model.Email };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return View(model);
        }  [Authorize(Roles = "admin")]
[HttpGet]

        public async Task<IActionResult> Edit(string id)
        {
              Console.WriteLine(id);
              Console.WriteLine("assasa");
            if (id == null)
            {
                return RedirectToAction("Index");

            }

            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {


                return View(new EditViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,


                });
            }
            Console.WriteLine("testpoint");
            return RedirectToAction("Index");

        }[Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditViewModel model)
        {
            if (id != model.Id)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.UserName;

                    var result = await _userManager.UpdateAsync(user);


                    if (result.Succeeded && !string.IsNullOrEmpty(model.Password))
                    {
                        await _userManager.RemovePasswordAsync(user);
                        await _userManager.AddPasswordAsync(user, model.Password);
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
        }[Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction("Index");

        }
        [AllowAnonymous]
        public async Task<IActionResult> Detailed(string id)
        {

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new EditViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Full_Name = user.Full_Name,
                Adress = user.Adress,
                Job = user.Job,
                age = user.age,
                gender = user.gender,
                bio = user.bio,
                hobby = user.hobby,
                nationality = user.nationality,
                marital_status = user.marital_status,

            };

            return View(model);
        }

    }


}