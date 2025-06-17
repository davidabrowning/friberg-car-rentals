using FribergCarRentals.Interfaces;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FribergCarRentals.Data
{
    public class DatabaseSeedingService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService _userService;
        public DatabaseSeedingService(RoleManager<IdentityRole> roleManager, IUserService userService)
        {
            _roleManager = roleManager;
            _userService = userService;
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
            IdentityUser? identityUser = _userService.GetAllIdentityUsersAsync()
                .Result.FirstOrDefault(iu => iu.UserName == "admin@admin.se");

            if (identityUser == null)
            {
                identityUser = await _userService.CreateIdentityUserAsync("admin@admin.se");
            }

            await _userService.MakeAdminAsync(identityUser);
        }
    }
}
