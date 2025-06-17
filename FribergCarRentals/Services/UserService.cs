using FribergCarRentals.Data;
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

        // General methods
        public async Task<IdentityUser?> DeleteIdentityUserAsync(string id)
        {
            await _adminService.DeleteAdminByIdentityUserIdAsync(id);
            await _customerService.DeleteCustomerByIdentityUserIdAsync(id);
            return await _identityUserService.DeleteAsync(id);
        }
        public async Task<Admin?> GetAdminAccountAsync(IdentityUser identityUser)
        {
            IEnumerable<Admin> admins = await _adminService.GetAllAsync();
            return admins.FirstOrDefault(a => a.IdentityUser.Id == identityUser.Id);
        }
        public async Task<Customer?> GetCustomerAccountAsync(IdentityUser identityUser)
        {
            IEnumerable<Customer> customers = await _customerService.GetAllAsync();
            return customers.FirstOrDefault(c => c.IdentityUser.Id == identityUser.Id);
        }
        public async Task<Admin?> MakeAdminAsync(IdentityUser identityUser)
        {
            if (await _identityUserService.IsAdmin(identityUser))
            {
                return await GetAdminAccountAsync(identityUser);
            }

            await _identityUserService.MakeAdmin(identityUser.UserName);
            Admin admin = new Admin() { IdentityUser = identityUser };
            return admin;
        }

        // IdentityUser methods
        public async Task<IdentityUser> CreateIdentityUserAsync(string username) => await _identityUserService.AddIdentityUserAsync(username);
        public async Task<IEnumerable<IdentityUser>> GetAllIdentityUsersAsync() => await _identityUserService.GetAllAsync();
        public async Task<IdentityUser?> GetCurrentSignedInIdentityUserAsync() => await _identityUserService.GetCurrentSignedInIdentityUserAsync();
        public async Task<IdentityUser?> GetIdentityUserByIdAsync(string id) => await _identityUserService.GetByIdAsync(id);
        public async Task<bool> IdentityUserExistsAsync(string id) => await _identityUserService.IdExistsAsync(id);
        public Task<bool> IdentityUsernameExistsAsync(string username) => _identityUserService.UsernameExistsAsync(username);
        public Task<bool> IsInRoleAsync(IdentityUser identityUser, string roleName) => _identityUserService.IsInRoleAsync(identityUser, roleName);
        public Task<IdentityUser> UpdateIdentityUserAsync(string id, string newUsername) => _identityUserService.UpdateUsernameAsync(id, newUsername);

        // Admin methods
        public async Task<bool> AdminIdExistsAsync(int id) => await _adminService.IdExistsAsync(id);
        public async Task<Admin> CreateAdminAsync(Admin admin) => await _adminService.CreateAsync(admin);
        public async Task<Admin?> DeleteAdminAsync(int id) => await _adminService.DeleteAsync(id);
        public async Task<Admin?> GetAdminByIdAsync(int id) => await _adminService.GetByIdAsync(id);
        public async Task<IEnumerable<Admin>> GetAllAdminsAsync() => await _adminService.GetAllAsync();
        public async Task<Admin> UpdateAdminAsync(Admin admin) => await _adminService.UpdateAsync(admin);

        // Customer methods
        public async Task<Customer> CreateCustomerAsync(Customer customer) => await _customerService.CreateAsync(customer);
        public async Task<bool> CustomerIdExistsAsync(int id) => await _customerService.IdExistsAsync(id);
        public async Task<Customer?> DeleteCustomerAsync(int id) => await _customerService.DeleteAsync(id);
        public async Task<IEnumerable<Customer>> GetAllCustomersAsync() => await _customerService.GetAllAsync();
        public async Task<Customer?> GetCustomerByIdAsync(int id) => await _customerService.GetByIdAsync(id);
        public async Task<Customer> UpdateCustomerAsync(Customer customer) => await _customerService.UpdateAsync(customer);
    }
}
