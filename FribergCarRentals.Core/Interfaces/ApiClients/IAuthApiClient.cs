namespace FribergCarRentals.Core.Interfaces.ApiClients
{
    public interface IAuthApiClient
    {
        Task<string?> GetCurrentSignedInUserIdAsync();
        Task<bool> IsAdminAsync(string userId);
        Task<bool> IsCustomerAsync(string userId);
        Task<bool> IsUserAsync(string userId);
        Task<string> GetUsernameByUserIdAsync(string userId);
        Task<List<string>> GetAllUserIdsAsync();
        Task CreateUserAsync(string username);
        Task UpdateUsernameAsync(string userId, string newUsername);
        Task DeleteUserAsync(string userId);
        Task<bool> IsInRoleAsync(string userId, string roleName);
        Task<int> GetAdminIdByUserId(string userId);
        Task<int> GetCustomerIdByUserId(string userId);
    }
}
