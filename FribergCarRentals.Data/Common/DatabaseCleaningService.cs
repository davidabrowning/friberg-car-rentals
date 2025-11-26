using FribergCarRentals.Core.Interfaces.Other;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;

namespace FribergCarRentals.Data
{
    public class DatabaseCleaningService : IDatabaseCleaner
    {
        private readonly ICrudService<Admin> _adminService;
        private readonly ICrudService<Customer> _customerService;
        public DatabaseCleaningService(ICrudService<Admin> adminService, ICrudService<Customer> customerService)
        {
            _adminService = adminService;
            _customerService = customerService;
        }
        public async Task CleanAsync()
        {
            await RemoveAdminsWithoutIdentityUsers();
            await RemoveCustomersWithoutIdentityUsers();
        }
        private async Task RemoveAdminsWithoutIdentityUsers()
        {
            IEnumerable<Admin> admins = await _adminService.GetAllAsync();
            foreach (Admin admin in admins)
            {
                if (admin.UserId == null)
                {
                    await _adminService.DeleteAsync(admin.Id);
                }
            }
        }
        private async Task RemoveCustomersWithoutIdentityUsers()
        {
            IEnumerable<Customer> customers = await _customerService.GetAllAsync();
            foreach (Customer customer in customers)
            {
                if (customer.UserId == null)
                {
                    await _customerService.DeleteAsync(customer.Id);
                }
            }
        }
    }
}
