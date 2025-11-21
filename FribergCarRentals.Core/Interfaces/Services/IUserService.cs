using FribergCarRentals.Core.Models;

namespace FribergCarRentals.Core.Interfaces.Services
{
    public interface IUserService
    {
        // IdentityUser
        Task<string> CreateUser(string username);
        Task<string?> GetCurrentUserId();
        Task<string?> GetUsernameByUserId(string userId);
        Task<string?> GetUserIdByUsername(string username);
        Task<string?> UpdateUsername(string userId, string newUsername);
        Task<string?> DeleteUserAsync(string userId);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> IsInRoleAsync(string userId, string roleName);
        Task<List<string>> GetAllUserIdsAsync();
        

        // Admin
        Task<Admin> CreateAdminAsync(Admin admin);
        Task<Admin?> GetAdminByAdminIdAsync(int id);
        Task<Admin?> GetAdminByUserIdAsync(string userId);
        Task<Admin> UpdateAdminAsync(Admin admin);
        Task<Admin?> DeleteAdminAsync(int id);

        // Customer
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<Customer?> GetCustomerByCustomerIdAsync(int id);
        Task<Customer?> GetCustomerByUserIdAsync(string userId);
        Task<Customer?> GetSignedInCustomer();
        Task<Customer> UpdateCustomerAsync(Customer customer);
        Task<Customer?> DeleteCustomerAsync(int id);
    }
}
