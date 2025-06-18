using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity;

namespace FribergCarRentals.Interfaces
{
    public interface IUserService
    {
        // Complex
        Task<Admin?> GetAdminAsync(IdentityUser identityUser);
        Task<Customer?> GetCustomerAsync(IdentityUser identityUser);
        Task<IdentityUser?> DeleteUserAsync(string id);

        // IdentityUser
        Task<IdentityUser?> GetCurrentSignedInIdentityUserAsync();
        Task<IdentityUser> CreateIdentityUserAsync(string username);
        Task<IdentityUser?> GetIdentityUserByIdAsync(string id);
        Task<IdentityUser> UpdateIdentityUserAsync(string id, string newUsername);
        Task<bool> IdentityUsernameExistsAsync(string username);
        Task<bool> IdentityUserExistsAsync(string id);
        Task<bool> IsInRoleAsync(IdentityUser identityUser, string roleName);
        Task<IEnumerable<IdentityUser>> GetAllIdentityUsersAsync();

        // Admin
        Task<Admin> CreateAdminAsync(Admin admin);
        Task<Admin?> GetAdminByIdAsync(int id);
        Task<Admin> UpdateAdminAsync(Admin admin);
        Task<bool> AdminIdExistsAsync(int id);
        Task<Admin?> DeleteAdminAsync(int id);

        // Customer
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<Customer> UpdateCustomerAsync(Customer customer);
        Task<Customer?> DeleteCustomerAsync(int id);
        Task<bool> CustomerIdExistsAsync(int id);


        
        
    }
}
