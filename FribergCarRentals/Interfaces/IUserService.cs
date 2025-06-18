using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity;

namespace FribergCarRentals.Interfaces
{
    public interface IUserService
    {
        // IdentityUser
        Task<IdentityUser> CreateUser(string username);
        Task<IdentityUser?> GetCurrentUser();
        Task<IdentityUser?> GetUserById(string id);
        Task<IdentityUser> UpdateUsername(string id, string newUsername);
        Task<IdentityUser?> DeleteIdentityUserAsync(string id);
        Task<bool> IdentityUsernameExistsAsync(string username);
        Task<bool> IsInRoleAsync(IdentityUser identityUser, string roleName);
        Task<IEnumerable<IdentityUser>> GetAllIdentityUsersAsync();
        

        // Admin
        Task<Admin> CreateAdminAsync(Admin admin);
        Task<Admin?> GetAdminByIdAsync(int id);
        Task<Admin?> GetAdminByUserAsync(IdentityUser identityUser);
        Task<Admin> UpdateAdminAsync(Admin admin);
        Task<Admin?> DeleteAdminAsync(int id);

        // Customer
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<Customer?> GetCustomerByUserAsync(IdentityUser identityUser);
        Task<Customer> UpdateCustomerAsync(Customer customer);
        Task<Customer?> DeleteCustomerAsync(int id);
    }
}
