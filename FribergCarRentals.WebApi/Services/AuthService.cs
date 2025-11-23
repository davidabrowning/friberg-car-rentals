using FribergCarRentals.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using FribergCarRentals.Core.Interfaces.Services;

namespace FribergCarRentals.WebApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private const string DefaultAdminEmail = "admin@admin.se";
        private const string DefaultPassword = "Abc123!";
        public const string RoleNameAdmin = "Admin";
        public const string RoleNameCustomer = "Customer";
        public const string RoleNameUser = "User";
        public AuthService(IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> IdExistsAsync(string id)
        {
            return await _userManager.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _userManager.Users.AnyAsync(u => u.UserName == username);
        }

        public async Task<string> AddUserAsync(string username)
        {
            IdentityUser identityUser = new IdentityUser() { UserName = username, Email = username };
            string initialPassword = DefaultPassword;
            await _userManager.CreateAsync(identityUser, initialPassword);
            await _userManager.AddToRoleAsync(identityUser, RoleNameUser);
            return identityUser.Id;
        }

        public async Task<string?> GetUsernameByUserIdAsync(string userId)
        {
            IdentityUser? identityUser = await GetIdentityUserByUserId(userId);
            if (identityUser == null)
            {
                return null;
            }
            return identityUser.UserName;
        }

        public async Task<string?> GetUserIdByUsernameAsync(string username)
        {
            IdentityUser? identityUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (identityUser == null)
            {
                return null;
            }
            return identityUser.Id;
        }

        public async Task<string?> UpdateUsernameAndReturnStringUserIdAsync(string userId, string newUsername)
        {
            IdentityUser? identityUser = await GetIdentityUserByUserId(userId);
            if (identityUser == null)
            {
                return null;
            }

            await _userManager.SetUserNameAsync(identityUser, newUsername);
            return userId;
        }

        public async Task<string?> DeleteByUserIdAsync(string userId)
        {
            IdentityUser? identityUser = await _userManager.Users.Where(iu => iu.Id == userId).FirstOrDefaultAsync();
            if (identityUser != null)
            {
                await _userManager.DeleteAsync(identityUser);
            }
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

        public async Task<string?> GetCurrentSignedInUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
            {
                return null;
            }
            IdentityUser? identityUser = await _userManager.GetUserAsync(user);
            if (identityUser == null)
            {
                return null;
            }
            return identityUser.Id;
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
            await _userManager.AddToRoleAsync(identityUser, roleName);
            await _signInManager.RefreshSignInAsync(identityUser);
            return userId;
        }

        public async Task<string?> RemoveFromRoleAsync(string userId, string roleName)
        {
            IdentityUser? identityUser = await GetIdentityUserByUserId(userId);
            await _userManager.RemoveFromRoleAsync(identityUser, roleName);
            await _signInManager.RefreshSignInAsync(identityUser);
            return userId;
        }

        private async Task<IdentityUser?> GetIdentityUserByUserId(string userId)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
