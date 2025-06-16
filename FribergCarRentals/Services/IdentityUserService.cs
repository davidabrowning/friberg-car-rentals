using FribergCarRentals.Data;
using FribergCarRentals.Interfaces;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentals.Services
{
    public class IdentityUserService : IIdentityUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<Admin> _adminRepository;
        private readonly IRepository<Customer> _customerRepository;
        private const string DefaultAdminEmail = "admin@admin.se";
        private const string DefaultPassword = "Abc123!";
        private const string RoleNameAdmin = "Admin";
        private const string RoleNameCustomer = "Customer";
        private const string RoleNameUser = "User";
        public IdentityUserService(UserManager<IdentityUser> userManager, IRepository<Admin> adminRepository, IRepository<Customer> customerRepository)
        {
            _userManager = userManager;
            _adminRepository = adminRepository;
            _customerRepository = customerRepository;
        }
        public async Task<IdentityUser> AddIdentityUserAsync(string username)
        {
            IdentityUser identityUser = new IdentityUser() { UserName = username, Email = username };
            string initialPassword = DefaultPassword;
            await _userManager.CreateAsync(identityUser, initialPassword);
            await _userManager.AddToRoleAsync(identityUser, RoleNameUser);
            return identityUser;
        }

        public async Task<IdentityUser?> DeleteAsync(string id)
        {
            IEnumerable<Admin> admins = await _adminRepository.GetAllAsync();
            Admin? admin = admins.Where(a => a.IdentityUser.Id == id).FirstOrDefault();
            if (admin != null)
            {
                await _adminRepository.DeleteAsync(admin.Id);
            }

            IEnumerable<Customer> customers = await _customerRepository.GetAllAsync();
            Customer? customer = customers.Where(c => c.IdentityUser.Id == id).FirstOrDefault();
            if (customer != null)
            {
                await _customerRepository.DeleteAsync(customer.Id);
            }

            IdentityUser? identityUser = await _userManager.Users.Where(iu => iu.Id == id).FirstOrDefaultAsync();
            if (identityUser != null)
            {
                await _userManager.DeleteAsync(identityUser);
            }

            return identityUser;
        }

        public async Task<List<IdentityUser>> GetAllAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<IdentityUser?> GetByEmailAsync(string email)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IdentityUser?> GetByIdAsync(string id)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> IdExistsAsync(string id)
        {
            return await _userManager.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> IsAdmin(IdentityUser identityUser)
        {
            return await IsInRoleAsync(identityUser, RoleNameAdmin);
        }

        public async Task<bool> IsCustomer(IdentityUser identityUser)
        {
            return await IsInRoleAsync(identityUser, RoleNameCustomer);
        }

        public async Task<bool> IsInRoleAsync(IdentityUser identityUser, string role)
        {
            return await _userManager.IsInRoleAsync(identityUser, role);
        }

        public async Task<IdentityUser?> UpdateUsernameAsync(string id, string newUsername)
        {
            IdentityUser? identityUser = await GetByIdAsync(id);
            if (identityUser == null)
            {
                return null;
            }
            await _userManager.SetUserNameAsync(identityUser, newUsername);
            return identityUser;
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _userManager.Users.AnyAsync(u => u.UserName == username);
        }

        public async Task<IdentityUser?> MakeAdmin(string username)
        {
            IdentityUser? user = await GetByEmailAsync(DefaultAdminEmail);
            if (user == null)
            {
                user = await AddIdentityUserAsync(DefaultAdminEmail);
            }

            if (await IsAdmin(user))
            {
                return user;
            }

            await _userManager.AddToRoleAsync(user, RoleNameAdmin);
            Admin admin = new Admin() { IdentityUser = user };
            await _adminRepository.AddAsync(admin);
            return user;
        }

        public Task<IdentityUser?> MakeCustomer(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<Admin?> GetAdminAccount(IdentityUser identityUser)
        {
            IEnumerable<Admin> admins = await _adminRepository.GetAllAsync();
            return admins.FirstOrDefault(a => a.IdentityUser.Id == identityUser.Id);
        }

        public async Task<Customer?> GetCustomerAccount(IdentityUser identityUser)
        {
            IEnumerable<Customer> customers = await _customerRepository.GetAllAsync();
            return customers.FirstOrDefault(c => c.IdentityUser.Id == identityUser.Id);
        }
    }
}
