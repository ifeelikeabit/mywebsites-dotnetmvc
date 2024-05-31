using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using MyGardenShip.Data;
using MyGardenShip.Models;

namespace MyGardenShip.Controllers
{
    //bu controller a erişmek için yetkilendirme gerektiğini belirtme
    public class ProductsController : Controller
    {

        private readonly IdentityContext _context;

        private UserManager<AppUser> _userManager;
        public ProductsController(IdentityContext context, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var productsData = await _context.Products.ToListAsync();
            var products = new List<ProductViewModel>();
            productsData.ForEach(product =>
            {
                string action = "Details";
                if (user != null)
                    if (product.Email == user?.Email)
                    {
                        action = "Edit";
                    }
                products.Add(new ProductViewModel { Action = action, Product = product });
            });
            return View(products);
        }
        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(Product model, IFormFile Image)
        {
            if (Image == null || Image.Length == 0)
            {
                return View(model);
            }
            byte[] imageBytes = new byte[(int)Image.Length];
            Image.OpenReadStream().Read(imageBytes, 0, (int)Image.Length);

            string base64Image = Convert.ToBase64String(imageBytes);
            model.Image = base64Image;
            var me = await _userManager.GetUserAsync(HttpContext.User);
            if (null != me)
                model.Email = me.Email;
            else
                return RedirectToAction("Login", "User");
            _context.Products.Add(model);
            me.ProductCount++;
            await _userManager.UpdateAsync(me);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) { return Redirect("/Products"); };
            var product = await _context.Products.FindAsync(id);
            return View(product);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null || (user.Email != product.Email && !roles.Contains("admin")))
            {
                return NotFound();
            }
            return View(product);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, string Email, Product model, IFormFile Image)
        {

            if (Image == null || Image.Length == 0)
            {
                return View(model);
            }
            byte[] imageBytes = new byte[(int)Image.Length];
            Image.OpenReadStream().Read(imageBytes, 0, (int)Image.Length);

            string base64Image = Convert.ToBase64String(imageBytes);
            model.Image = base64Image;
            var me = await _userManager.GetUserAsync(HttpContext.User);

            model.Email = Email;


            if (id !=
            model.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Products.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (!_context.Products.Any(p => p.Id == model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // product = await _context.Products.FindAsync(id);
                return View(
                    model);
            }
            return View(model);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null || (user.Email != product.Email && !roles.Contains("admin")))
            {
                return NotFound();
            }
            return View(product);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var product = await _context.Products.FindAsync(id);
            var producer = await _userManager.FindByEmailAsync(product.Email);
            var roles = await _userManager.GetRolesAsync(user);
            if (product == null || (user.Email != product.Email && !roles.Contains("admin")))
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            producer.ProductCount--;
            await _userManager.UpdateAsync(producer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }

}