namespace FribergCarRentals.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<bool> IdExistsAsync(string id);
        Task<bool> UsernameExistsAsync(string username);
        Task<string?> AuthenticateUserAsync(string username, string password);
        Task<string?> CreateUserWithPasswordAsync(string username, string password);
        Task<string?> GetUsernameAsync(string userId);
        Task<string?> GetUserIdAsync(string username);
        Task<string?> UpdateUsernameAsync(string id, string newUsername);
        Task<string?> DeleteAsync(string id);
        Task<List<string>> GetAllUserIdsAsync();
        Task<bool> RoleExistsAsync(string roleName);
        Task<List<string>> GetAllRolesAsync();
        Task<List<string>> GetRolesAsync(string userId);
        Task<string?> CreateRoleAsync(string roleName);
        Task<bool> IsInRoleAsync(string userId, string roleName);
        Task<string?> AddToRoleAsync(string userId, string roleName);
        Task<string?> RemoveFromRoleAsync(string userId, string roleName);
    }
}
