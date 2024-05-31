using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MyGardenShip.Data
{
    public static class IdentitySeedData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Admin_123";
        public static async void IdentityTestUser(IApplicationBuilder app)
        {

            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<IdentityContext>();
            if (context.Database.GetAppliedMigrations().Any())
            {
                context.Database.Migrate();
            }
            // await context.Products.ForEachAsync(p => context.Products.Remove(p));
            // await context.SaveChangesAsync();

            var userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
            var role = await roleManager.FindByNameAsync("admin");
            if (role == null)
            {
                role = new AppRole
                {
                    Name = "admin",
                };
                await roleManager.CreateAsync(role);
            }
            var user = await userManager.FindByNameAsync(adminUser);
            if (user == null)
            {
                user = new AppUser
                {
                    FullName = "Admin",
                    UserName = adminUser,
                    Email = "admin@gmail.com",
                };
                await userManager.CreateAsync(user, adminPassword);
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                await userManager.ConfirmEmailAsync(user, token);
                await userManager.AddToRoleAsync(user, "admin");

            }else{

                 if(!user.EmailConfirmed){
  user.EmailConfirmed = true;
await userManager.UpdateAsync(user);

                 }
                var roles = await userManager.GetRolesAsync(user);
                if(!roles.Contains("admin")){
                      await userManager.AddToRoleAsync(user, "admin");
                }
            }
           
        }
    }
}