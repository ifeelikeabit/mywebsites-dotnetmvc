using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mowbank.Data;

namespace mowbank.Models
{

    public static class IdentitySeedData
    {

        private const string adminUser = "admin";
        private const string adminPassword = "Admin_123";

        public static async void IdentityTestUser(IApplicationBuilder app)
        {

            var context = app.ApplicationServices.CreateAsyncScope().ServiceProvider.GetRequiredService<IdentityContext>();
            if (context.Database.GetAppliedMigrations().Any())
            {
                context.Database.Migrate();
            }
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
                    UserName = adminUser,
                    Email = "admin@admin",
                    PhoneNumber = "4444444"
                };
                await userManager.CreateAsync(user, adminPassword);
                await userManager.AddToRoleAsync(user, "admin");
            }
            var roles = await userManager.GetRolesAsync(user);
            if (!roles.Contains("admin"))
            {

                await userManager.AddToRoleAsync(user, "admin");
            }
        }
    }
}
