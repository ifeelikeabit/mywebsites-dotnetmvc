using mowbank.Models;
using mowbank.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace mowbank.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public ProfileController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (id != null && User.IsInRole("admin"))
            {

                user = await _userManager.FindByIdAsync(id);
            }

            if (user == null)
            {
                return RedirectToAction("Create", "Account");
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
                salary = user.salary,
                marital_status = user.marital_status,

            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.UserName;
                    user.Full_Name = model.Full_Name;
                    user.Adress = model.Adress;
                    user.Job = model.Job;
                    user.age = model.age;
                    user.gender = model.gender;
                    user.bio = model.bio;
                    user.hobby = model.hobby;
                    user.nationality = model.nationality;
                    user.salary = model.salary;
                    user.marital_status = model.marital_status;


                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(model.Password))
                        {
                            var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                            if (removePasswordResult.Succeeded)
                            {
                                var addPasswordResult = await _userManager.AddPasswordAsync(user, model.Password);
                                if (!addPasswordResult.Succeeded)
                                {
                                    foreach (var err in addPasswordResult.Errors)
                                    {
                                        ModelState.AddModelError("", err.Description);
                                    }
                                    return View(model);
                                }
                            }
                            else
                            {
                                foreach (var err in removePasswordResult.Errors)
                                {
                                    ModelState.AddModelError("", err.Description);
                                }
                                return View(model);
                            }
                        }
                        return RedirectToAction("Detailed");
                    }
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Detailed()
        {
            // if (User.Identity.Name == "admin")
            // {
            //     return RedirectToAction("Edit", "Users");

            // }


            var user = await _userManager.GetUserAsync(User);
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
                salary = user.salary,
                marital_status = user.marital_status,

            };

            return View(model);
        }
    }
}
