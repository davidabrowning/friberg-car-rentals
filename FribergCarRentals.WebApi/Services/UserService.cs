using FribergCarRentals.Data;
using FribergCarRentals.Core.Models;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Core.Interfaces.Services;

namespace FribergCarRentals.WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IAuthService _authService;
        private readonly IAdminService _adminService;
        private readonly ICustomerService _customerService;
        public UserService(IAuthService authService, IAdminService adminService, ICustomerService customerService)
        {
            _authService = authService;
            _adminService = adminService;
            _customerService = customerService;
        }

        // General methods
        public async Task<Admin?> GetAdminByUserIdAsync(string userId)
        {
            IEnumerable<Admin> admins = await _adminService.GetAllAsync();
            return admins.FirstOrDefault(a => a.UserId == userId);
        }
        public async Task<Customer?> GetCustomerByUserIdAsync(string userId)
        {
            IEnumerable<Customer> customers = await _customerService.GetAllAsync();
            return customers.FirstOrDefault(c => c.UserId == userId);
        }

        // User methods
        public async Task<string> CreateUser(string username) =>
            await _authService.AddUserAsync(username);
        public async Task<string?> GetCurrentUserId() =>
            await _authService.GetCurrentSignedInUserId();
        public async Task<bool> UsernameExistsAsync(string username) =>
            await _authService.UsernameExistsAsync(username);
        public async Task<bool> IsInRoleAsync(string userId, string roleName) => 
            await _authService.IsInRoleAsync(userId, roleName);
        public async Task<string?> UpdateUsername(string userId, string newUsername) =>
            await _authService.UpdateUsernameAndReturnStringUserIdAsync(userId, newUsername);
            

        // Admin methods
        public async Task<Admin> CreateAdminAsync(Admin admin)
        {
            await _authService.AddToRoleAsync(admin.UserId, AuthService.RoleNameAdmin);
            await _adminService.CreateAsync(admin);
            return admin;
        }
        public async Task<Admin?> GetAdminByAdminIdAsync(int id) => 
            await _adminService.GetByIdAsync(id);
        public async Task<Admin> UpdateAdminAsync(Admin admin) => 
            await _adminService.UpdateAsync(admin);
        public async Task<Admin?> DeleteAdminAsync(int adminId)
        {
            Admin? admin = await GetAdminByAdminIdAsync(adminId);
            if (admin == null)
            {
                return null;
            }

            await _authService.RemoveFromRoleAsync(admin.UserId, AuthService.RoleNameAdmin);
            return await _adminService.DeleteAsync(admin.Id);
        }

        // Customer methods
        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            await _authService.AddToRoleAsync(customer.UserId, AuthService.RoleNameCustomer);
            await _customerService.CreateAsync(customer);
            return customer;
        }
        public async Task<Customer?> DeleteCustomerAsync(int id)
        {
            Customer? customer = await GetCustomerByCustomerIdAsync(id);
            if (customer == null)
            {
                return null;
            }

            await _authService.RemoveFromRoleAsync(customer.UserId, AuthService.RoleNameCustomer);
            await _customerService.DeleteAsync(customer.Id);
            return customer;
        }
        public async Task<Customer?> GetCustomerByCustomerIdAsync(int id) => 
            await _customerService.GetByIdAsync(id);
        public async Task<Customer> UpdateCustomerAsync(Customer customer) => 
            await _customerService.UpdateAsync(customer);

        public async Task<Customer?> GetSignedInCustomer()
        {
            string? userId = await GetCurrentUserId();
            if (userId == null)
            {
                return null;
            }

            Customer? customer = await GetCustomerByUserIdAsync(userId);
            return customer;
        }

        public async Task<string?> DeleteUserAsync(string userId)
        {
            Admin? admin = await GetAdminByUserIdAsync(userId);
            if (admin != null)
            {
                await DeleteAdminAsync(admin.Id);
            }

            Customer? customer = await GetCustomerByUserIdAsync(userId);
            if (customer != null)
            {
                await DeleteCustomerAsync(customer.Id);
            }

            await _authService.DeleteByUserIdAsync(userId);

            return userId;
        }

        public Task<string?> GetUserIdByUsername(string username)
        {
            return _authService.GetUserIdByUsernameAsync(username);
        }

        public Task<List<string>> GetAllUserIdsAsync()
        {
            return _authService.GetAllUserIdsAsync();
        }

        public Task<string?> GetUsernameByUserId(string userId)
        {
            return _authService.GetUsernameByUserIdAsync(userId);
        }
    }
}
