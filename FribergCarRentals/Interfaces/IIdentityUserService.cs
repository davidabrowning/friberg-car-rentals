using FribergCarRentals.Models;
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
        Task<bool> IsInRoleAsync(IdentityUser identityUser, string role);
        Task<IdentityUser?> GetByEmailAsync(string email);
        Task<bool> IsAdmin(IdentityUser identityUser);
        Task<bool> IsCustomer(IdentityUser identityUser);
        Task<IdentityUser?> MakeAdmin(string username);
        Task<IdentityUser?> MakeCustomer(string username);
        Task<IdentityUser?> GetCurrentSignedInIdentityUserAsync();
        Task<IdentityUser?> RemoveAdmin(string username);
        Task<IdentityUser?> RemoveCustomer(string username);
    }
}
