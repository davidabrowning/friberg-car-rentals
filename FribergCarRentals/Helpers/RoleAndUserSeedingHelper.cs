using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentals.Helpers
{
    public static class RoleAndUserSeedingHelper
    {
        public static async Task SeedRolesAndDefaultAdminUser(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                await SeedRoles(scope);
                await SeedDefaultAdminUser(scope);
                await SeedAllUsersAsUserRole(scope);
            }
        }

        private static async Task SeedRoles(IServiceScope scope)
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task SeedDefaultAdminUser(IServiceScope scope)
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByEmailAsync("admin@admin.se");

            if (user != null && !(await userManager.IsInRoleAsync(user, "Admin")))
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }

        private static async Task SeedAllUsersAsUserRole(IServiceScope scope)
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            List<ApplicationUser> allUsers = await userManager.Users.ToListAsync();

            foreach (var user in allUsers)
            {
                await userManager.AddToRoleAsync(user, "User");
            }
        }
    }
}
