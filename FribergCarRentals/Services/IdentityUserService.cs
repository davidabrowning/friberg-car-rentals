using FribergCarRentals.Interfaces;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentals.Services
{
    public class IdentityUserService : IIdentityUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        private const string DefaultAdminEmail = "admin@admin.se";
        private const string DefaultPassword = "Abc123!";
        public const string RoleNameAdmin = "Admin";
        public const string RoleNameCustomer = "Customer";
        public const string RoleNameUser = "User";
        public IdentityUserService(IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<IdentityUser> AddIdentityUserAsync(string username)
        {
            IdentityUser identityUser = new IdentityUser() { UserName = username, Email = username };
            string initialPassword = DefaultPassword;
            await _userManager.CreateAsync(identityUser, initialPassword);
            await _userManager.AddToRoleAsync(identityUser, RoleNameUser);
            return identityUser;
        }

        public async Task<IdentityUser?> DeleteAsync(string id)
        {
            IdentityUser? identityUser = await _userManager.Users.Where(iu => iu.Id == id).FirstOrDefaultAsync();
            if (identityUser != null)
            {
                await _userManager.DeleteAsync(identityUser);
            }
            return identityUser;
        }

        public async Task<List<IdentityUser>> GetAllAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<IdentityUser?> GetByEmailAsync(string email)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IdentityUser?> GetByIdAsync(string id)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> IdExistsAsync(string id)
        {
            return await _userManager.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> IsInRoleAsync(IdentityUser identityUser, string role)
        {
            return await _userManager.IsInRoleAsync(identityUser, role);
        }

        public async Task<IdentityUser?> UpdateUsernameAsync(string id, string newUsername)
        {
            IdentityUser? identityUser = await GetByIdAsync(id);
            if (identityUser == null)
            {
                return null;
            }
            await _userManager.SetUserNameAsync(identityUser, newUsername);
            return identityUser;
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _userManager.Users.AnyAsync(u => u.UserName == username);
        }

        public async Task<IdentityUser?> GetCurrentSignedInIdentityUserAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
            {
                return null;
            }
            return await _userManager.GetUserAsync(user);
        }

        public async Task<IdentityUser?> AddToRoleAsync(IdentityUser identityUser, string roleName)
        {
            await _userManager.AddToRoleAsync(identityUser, roleName);
            return identityUser;
        }

        public async Task<IdentityUser?> RemoveFromRoleAsync(IdentityUser identityUser, string roleName)
        {
            await _userManager.RemoveFromRoleAsync(identityUser, roleName);
            return identityUser;
        }
    }
}
