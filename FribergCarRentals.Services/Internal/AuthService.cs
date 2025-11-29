using FribergCarRentals.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FribergCarRentals.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<bool> IdExistsAsync(string id)
        {
            return await _userManager.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _userManager.Users.AnyAsync(u => u.UserName == username);
        }

        public async Task<string?> GetUsernameAsync(string userId)
        {
            IdentityUser? identityUser = await GetIdentityUserByUserId(userId);
            if (identityUser == null)
                return null;
            return identityUser.UserName;
        }

        public async Task<string?> GetUserIdAsync(string username)
        {
            IdentityUser? identityUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (identityUser == null)
                return null;
            return identityUser.Id;
        }

        public async Task<string?> UpdateUsernameAsync(string userId, string newUsername)
        {
            IdentityUser? identityUser = await GetIdentityUserByUserId(userId);
            if (identityUser == null)
                return null;

            await _userManager.SetUserNameAsync(identityUser, newUsername);
            return userId;
        }

        public async Task<string?> DeleteAsync(string userId)
        {
            IdentityUser? identityUser = await _userManager.Users.Where(iu => iu.Id == userId).FirstOrDefaultAsync();
            if (identityUser == null)
                return null;
            await _userManager.DeleteAsync(identityUser);
            return userId;
        }

        public async Task<List<string>> GetAllUserIdsAsync()
        {
            List<string> userIds = new();
            List<IdentityUser> identityUsers = await _userManager.Users.ToListAsync();
            foreach (IdentityUser identityUser in identityUsers)
            {
                userIds.Add(identityUser.Id);
            }
            return userIds;
        }

        public async Task<bool> IsInRoleAsync(string userId, string roleName)
        {
            IdentityUser? identityUser = await GetIdentityUserByUserId(userId);
            if (identityUser == null)
            {
                return false;
            }
            return await _userManager.IsInRoleAsync(identityUser, roleName);
        }

        public async Task<string?> AddToRoleAsync(string userId, string roleName)
        {
            IdentityUser? identityUser = await GetIdentityUserByUserId(userId);
            if (identityUser == null)
                return null;
            await _userManager.AddToRoleAsync(identityUser, roleName);
            return userId;
        }

        public async Task<string?> RemoveFromRoleAsync(string userId, string roleName)
        {
            IdentityUser? identityUser = await GetIdentityUserByUserId(userId);
            if (identityUser == null)
                return null;
            await _userManager.RemoveFromRoleAsync(identityUser, roleName);
            return userId;
        }

        private async Task<IdentityUser?> GetIdentityUserByUserId(string userId)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<string?> AuthenticateUserAsync(string username, string password)
        {
            IdentityUser? identityUser = await _userManager.FindByNameAsync(username);
            if (identityUser == null)
            {
                return null;
            }

            SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(identityUser, password, false);
            if (!signInResult.Succeeded)
            {
                return null;
            }
            return identityUser.Id;
        }

        public async Task<string?> CreateUserWithPasswordAsync(string username, string password)
        {
            IdentityUser identityUser = new() { UserName = username };
            IdentityResult identityResult = await _userManager.CreateAsync(identityUser, password);
            if (!identityResult.Succeeded)
            {
                return null;
            }
            return identityUser.Id;
        }

        public async Task<List<string>> GetAllRolesAsync()
        {
            List<IdentityRole> identityRoles = await _roleManager.Roles.ToListAsync();
            List<string> roleNames = new();
            foreach (IdentityRole identityRole in identityRoles)
            {
                if (identityRole.Name == null)
                    continue;
                roleNames.Add(identityRole.Name);
            }
            return roleNames;
        }

        public async Task<List<string>> GetRolesAsync(string userId)
        {
            IdentityUser? identityUser = await _userManager.FindByIdAsync(userId);
            if (identityUser == null)
            {
                return new List<string>();
            }
            IEnumerable<string> roles = await _userManager.GetRolesAsync(identityUser);
            return roles.ToList();
        }

        public Task<bool> RoleExistsAsync(string roleName)
        {
            return _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<string?> CreateRoleAsync(string roleName)
        {
            IdentityRole identityRole = new(roleName);
            IdentityResult identityResult = await _roleManager.CreateAsync(identityRole);
            if (!identityResult.Succeeded)
                return null;
            return roleName;
        }
    }
}
