namespace FribergCarRentals.Interfaces
{
    public interface IAuthService
    {
        Task<bool> IdExistsAsync(string id);
        Task<bool> UsernameExistsAsync(string username);
        Task<string> AddUserAsync(string username);
        Task<string?> GetUsernameByUserIdAsync(string username);
        Task<string?> GetUserIdByUsernameAsync(string id);
        Task<string?> UpdateUsernameAndReturnStringUserIdAsync(string id, string newUsername);
        Task<string?> DeleteByUserIdAsync(string id);
        Task<List<string>> GetAllUserIdsAsync();
        Task<string?> GetCurrentSignedInUserId();
        Task<bool> IsInRoleAsync(string userId, string roleName);
        Task<string?> AddToRoleAsync(string userId, string roleName);
        Task<string?> RemoveFromRoleAsync(string userId, string roleName);
    }
}
