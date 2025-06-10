using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentals.Data
{
    public class DatabaseCleaningService
    {
        private readonly IRepository<Admin> _adminRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly UserManager<IdentityUser> _userManager;
        public DatabaseCleaningService(IRepository<Admin> adminRepository, IRepository<Customer> customerRepository, UserManager<IdentityUser> userManager)
        {
            _adminRepository = adminRepository;
            _customerRepository = customerRepository;
            _userManager = userManager;
        }
        public async Task Go()
        {
            await RemoveAdminsWithoutIdentityUsers();
            await RemoveCustomersWithoutIdentityUsers();
            await RemoveAdminRoleFromUsersWithoutAdminAccount();
            await RemoveCustomerRoleFromUsersWithoutCustomerAccount();
        }
        private async Task RemoveAdminsWithoutIdentityUsers()
        {
            IEnumerable<Admin> admins = await _adminRepository.GetAll();
            foreach (Admin admin in admins)
            {
                if (admin.IdentityUser == null)
                {
                    await _adminRepository.Delete(admin.Id);
                }
            }
        }
        private async Task RemoveCustomersWithoutIdentityUsers()
        {
            IEnumerable<Customer> customers = await _customerRepository.GetAll();
            foreach (Customer customer in customers)
            {
                if (customer.IdentityUser == null)
                {
                    await _customerRepository.Delete(customer.Id);
                }
            }
        }
        private async Task RemoveAdminRoleFromUsersWithoutAdminAccount()
        {
            IEnumerable<Admin> admins = await _adminRepository.GetAll();
            foreach (IdentityUser identityUser in await _userManager.Users.ToListAsync())
            {
                if (!admins.Any(a => a.IdentityUser == identityUser))
                {
                    await _userManager.RemoveFromRoleAsync(identityUser, "Admin");
                }
            }
        }
        private async Task RemoveCustomerRoleFromUsersWithoutCustomerAccount()
        {
            IEnumerable<Customer> customers = await _customerRepository.GetAll();
            foreach (IdentityUser identityUser in await _userManager.Users.ToListAsync())
            {
                if (!customers.Any(a => a.IdentityUser == identityUser))
                {
                    await _userManager.RemoveFromRoleAsync(identityUser, "Customer");
                }
            }
        }
    }
}
