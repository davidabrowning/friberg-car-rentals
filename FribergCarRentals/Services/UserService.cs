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
            IdentityUser? identityUser = await GetUserById(id);
            if (identityUser == null) {
                return null;
            }

            Admin? admin = await GetAdminByUserAsync(identityUser);
            if (admin != null)
            {
                await DeleteAdminAsync(admin.Id);
            }

            Customer? customer = await GetCustomerByUserAsync(identityUser);
            if (customer != null)
            {
                await DeleteCustomerAsync(customer.Id);
            }

            await _identityUserService.DeleteAsync(id);

            return identityUser;
        }
        public async Task<Admin?> GetAdminByUserAsync(IdentityUser identityUser)
        {
            IEnumerable<Admin> admins = await _adminService.GetAllAsync();
            return admins.FirstOrDefault(a => a.IdentityUser.Id == identityUser.Id);
        }
        public async Task<Customer?> GetCustomerByUserAsync(IdentityUser identityUser)
        {
            IEnumerable<Customer> customers = await _customerService.GetAllAsync();
            return customers.FirstOrDefault(c => c.IdentityUser.Id == identityUser.Id);
        }

        // IdentityUser methods
        public async Task<IdentityUser> CreateUser(string username) =>
            await _identityUserService.AddIdentityUserAsync(username);
        public async Task<IEnumerable<IdentityUser>> GetAllIdentityUsersAsync() =>
            await _identityUserService.GetAllAsync();
        public async Task<IdentityUser?> GetCurrentUser() =>
            await _identityUserService.GetCurrentSignedInIdentityUserAsync();
        public async Task<IdentityUser?> GetUserById(string id) =>
            await _identityUserService.GetByIdAsync(id);
        public async Task<bool> IdentityUsernameExistsAsync(string username) =>
            await _identityUserService.UsernameExistsAsync(username);
        public async Task<bool> IsInRoleAsync(IdentityUser identityUser, string roleName) => 
            await _identityUserService.IsInRoleAsync(identityUser, roleName);
        public async Task<IdentityUser?> UpdateUsername(string id, string newUsername) => 
            await _identityUserService.UpdateUsernameAsync(id, newUsername);

        // Admin methods
        public async Task<Admin> CreateAdminAsync(Admin admin)
        {
            await _identityUserService.AddToRoleAsync(admin.IdentityUser, IdentityUserService.RoleNameAdmin);
            await _adminService.CreateAsync(admin);
            return admin;
        }
        public async Task<Admin?> GetAdminByIdAsync(int id) => 
            await _adminService.GetByIdAsync(id);
        public async Task<Admin> UpdateAdminAsync(Admin admin) => 
            await _adminService.UpdateAsync(admin);
        public async Task<Admin?> DeleteAdminAsync(int adminId)
        {
            Admin? admin = await GetAdminByIdAsync(adminId);
            if (admin == null)
            {
                return null;
            }

            await _identityUserService.RemoveFromRoleAsync(admin.IdentityUser, IdentityUserService.RoleNameAdmin);
            return await _adminService.DeleteAsync(admin.Id);
        }

        // Customer methods
        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            await _identityUserService.AddToRoleAsync(customer.IdentityUser, IdentityUserService.RoleNameCustomer);
            await _customerService.CreateAsync(customer);
            return customer;
        }
        public async Task<Customer?> DeleteCustomerAsync(int id)
        {
            Customer? customer = await GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return null;
            }

            await _identityUserService.RemoveFromRoleAsync(customer.IdentityUser, IdentityUserService.RoleNameCustomer);
            await _customerService.DeleteAsync(customer.Id);
            return customer;
        }
        public async Task<Customer?> GetCustomerByIdAsync(int id) => 
            await _customerService.GetByIdAsync(id);
        public async Task<Customer> UpdateCustomerAsync(Customer customer) => 
            await _customerService.UpdateAsync(customer);
    }
}
