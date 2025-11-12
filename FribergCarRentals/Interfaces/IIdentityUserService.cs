using FribergCarRentals.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace FribergCarRentals.Interfaces
{
    public interface IIdentityUserService
    {
        Task<IdentityUser> AddIdentityUserAsync(string username);
        Task<bool> IdExistsAsync(string id);
        Task<bool> UsernameExistsAsync(string username);
        Task<IdentityUser?> GetByIdAsync(string id);
        Task<IdentityUser?> UpdateUsernameAsync(string id, string newUsername);
        Task<IdentityUser?> DeleteAsync(string id);
        Task<List<IdentityUser>> GetAllAsync();
        Task<IdentityUser?> GetByEmailAsync(string email);        
        Task<IdentityUser?> GetCurrentSignedInIdentityUserAsync();
        Task<bool> IsInRoleAsync(IdentityUser identityUser, string roleName);
        Task<IdentityUser?> AddToRoleAsync(IdentityUser identityUser, string roleName);
        Task<IdentityUser?> RemoveFromRoleAsync(IdentityUser identityUser, string roleName);
    }
}
