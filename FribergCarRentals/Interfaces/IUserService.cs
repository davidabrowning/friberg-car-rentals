using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity;

namespace FribergCarRentals.Interfaces
{
    public interface IUserService
    {
        Task<IdentityUser?> GetCurrentSignedInIdentityUserAsync();
        Task<Admin> CreateAdminAsync(Admin admin);
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<IdentityUser> CreateIdentityUserAsync(string username);
        Task<Admin?> GetAdminByIdAsync(int id);
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<IdentityUser?> GetIdentityUserByIdAsync(string id);
        Task<Admin> UpdateAdminAsync(Admin admin);
        Task<Customer> UpdateCustomerAsync(Customer customer);
        Task<IdentityUser> UpdateIdentityUserAsync(string id, string newUsername);
        Task<Admin?> DeleteAdminAsync(int id);
        Task<Customer?> DeleteCustomerAsync(int id);
        Task<IdentityUser?> DeleteIdentityUserAsync(string id);
        Task<IEnumerable<Admin>> GetAllAdminsAsync();
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<IEnumerable<IdentityUser>> GetAllIdentityUsersAsync();
        Task<bool> AdminIdExistsAsync(int id);
        Task<bool> CustomerIdExistsAsync(int id);
        Task<bool> IdentityUserExistsAsync(string id);
        Task<bool> IsInRoleAsync(IdentityUser identityUser, string roleName);
        Task<Admin?> GetAdminAccountAsync(IdentityUser identityUser);
        Task<Customer?> GetCustomerAccountAsync(IdentityUser identityUser);
        Task<bool> IdentityUsernameExistsAsync(string username);
        Task<Admin?> MakeAdminAsync(IdentityUser identityUser);
    }
}
