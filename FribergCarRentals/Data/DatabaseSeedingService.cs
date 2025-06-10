using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentals.Data
{
    public class DatabaseSeedingService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<Admin> _adminRepository;
        public DatabaseSeedingService(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IRepository<Admin> adminRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _adminRepository = adminRepository;
        }
        public async Task Go()
        {
            await SeedRoles();
            await SeedDefaultAdminUser();
            await SeedAllUsersAsUserRole();
        }

        private async Task SeedRoles()
        {
            string[] roles = { "Admin", "Customer", "User" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private async Task SeedDefaultAdminUser()
        {
            IdentityUser user = await _userManager.FindByEmailAsync("admin@admin.se");
            if (user != null && !(await _userManager.IsInRoleAsync(user, "Admin")))
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                Admin admin = new Admin() { IdentityUser = user };
                await _adminRepository.Add(admin);
            }
        }

        private async Task SeedAllUsersAsUserRole()
        {
            List<IdentityUser> allUsers = await _userManager.Users.ToListAsync();
            foreach (IdentityUser user in allUsers)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
        }
    }
}
