using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace bordro.Data
{
    public static class IdentitySeedData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Admin_123";

        public static async Task IdentityTestUser(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IdentityContext>();
                if (context.Database.GetPendingMigrations().Any())
                {
                    await context.Database.MigrateAsync();
                }

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

                var adminRole = await roleManager.FindByNameAsync("admin");
                var companyRole = await roleManager.FindByNameAsync("Company");

                if (adminRole == null)
                {
                    adminRole = new AppRole
                    {
                        Name = "admin",
                    };
                    await roleManager.CreateAsync(adminRole);
                }

                if (companyRole == null)
                {
                    companyRole = new AppRole
                    {
                        Name = "Company",
                    };
                    await roleManager.CreateAsync(companyRole);
                }

                var user = await userManager.FindByNameAsync(adminUser);
                if (user == null)
                {
                    user = new AppUser
                    {
                        UserName = adminUser,
                        Email = "admin@gmail.com",
                    };
                    var result = await userManager.CreateAsync(user, adminPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "admin");
                    }
                }
                else
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (!roles.Contains("admin"))
                    {
                        await userManager.AddToRoleAsync(user, "admin");
                    }
                }
            }
        }
    }
}
