using FribergCarRentals.Interfaces;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FribergCarRentals.Services
{
    public class UserService : IUserService
    {
        private readonly IIdentityUserService _identityUserService;
        private readonly IAdminService _adminService;
        private readonly ICustomerService _customerService;
        public UserService(IIdentityUserService identityUserService, IAdminService adminService, ICustomerService customerService)
        {
            _identityUserService = identityUserService;
            _adminService = adminService;
            _customerService = customerService;
        }
        public async Task<bool> AdminIdExistsAsync(int id) => await _adminService.IdExistsAsync(id);
        public async Task<Admin> CreateAdminAsync(Admin admin) => await _adminService.CreateAsync(admin);
        public async Task<Customer> CreateCustomerAsync(Customer customer) => await _customerService.CreateAsync(customer);
        public async Task<IdentityUser> CreateIdentityUserAsync(string username) => await _identityUserService.AddIdentityUserAsync(username);
        public async Task<bool> CustomerIdExistsAsync(int id) => await _customerService.IdExistsAsync(id);
        public async Task<Admin?> DeleteAdminAsync(int id) => await _adminService.DeleteAsync(id);
        public async Task<Customer?> DeleteCustomerAsync(int id) => await _customerService.DeleteAsync(id);
        public async Task<IdentityUser?> DeleteIdentityUserAsync(string id) => await _identityUserService.DeleteAsync(id);
        public async Task<Admin?> GetAdminAccountAsync(IdentityUser identityUser) => await _identityUserService.GetAdminAccountAsync(identityUser);
        public async Task<Admin?> GetAdminByIdAsync(int id) => await _adminService.GetByIdAsync(id);
        public async Task<IEnumerable<Admin>> GetAllAdminsAsync() => await _adminService.GetAllAsync();
        public async Task<IEnumerable<Customer>> GetAllCustomersAsync() => await _customerService.GetAllAsync();
        public async Task<IEnumerable<IdentityUser>> GetAllIdentityUsersAsync() => await _identityUserService.GetAllAsync();
        public async Task<IdentityUser?> GetCurrentSignedInIdentityUserAsync() => await _identityUserService.GetCurrentSignedInIdentityUserAsync();
        public async Task<Customer?> GetCustomerAccountAsync(IdentityUser identityUser) => await _identityUserService.GetCustomerAccountAsync(identityUser);
        public async Task<Customer?> GetCustomerByIdAsync(int id) => await _customerService.GetByIdAsync(id);
        public async Task<IdentityUser?> GetIdentityUserByIdAsync(string id) => await _identityUserService.GetByIdAsync(id);
        public async Task<bool> IdentityUserExistsAsync(string id) => await _identityUserService.IdExistsAsync(id);
        public Task<bool> IdentityUsernameExistsAsync(string username) => _identityUserService.UsernameExistsAsync(username);
        public Task<bool> IsInRoleAsync(IdentityUser identityUser, string roleName) => _identityUserService.IsInRoleAsync(identityUser, roleName);
        public async Task<Admin> UpdateAdminAsync(Admin admin) => await _adminService.UpdateAsync(admin);
        public async Task<Customer> UpdateCustomerAsync(Customer customer) => await _customerService.UpdateAsync(customer);
        public Task<IdentityUser> UpdateIdentityUserAsync(string id, string newUsername) => _identityUserService.UpdateUsernameAsync(id, newUsername);
    }
}
