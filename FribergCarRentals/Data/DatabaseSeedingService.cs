using FribergCarRentals.Interfaces;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentals.Data
{
    public class DatabaseSeedingService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IIdentityUserService _identityUserService;
        private readonly IRepository<Admin> _adminRepository;
        public DatabaseSeedingService(RoleManager<IdentityRole> roleManager, IIdentityUserService identityUserService, IRepository<Admin> adminRepository)
        {
            _roleManager = roleManager;
            _identityUserService = identityUserService;
            _adminRepository = adminRepository;
        }
        public async Task Go()
        {
            await SeedRoles();
            await SeedDefaultAdminUser();
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
            await _identityUserService.MakeAdmin("admin@admin.se");
        }
    }
}
