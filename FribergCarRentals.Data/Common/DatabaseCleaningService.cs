using FribergCarRentals.Core.Interfaces.Other;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;

namespace FribergCarRentals.Data
{
    public class DatabaseCleaningService : IDatabaseCleaner
    {
        private readonly IBasicCRUDService<Admin> _adminService;
        private readonly IBasicCRUDService<Customer> _customerService;
        public DatabaseCleaningService(IBasicCRUDService<Admin> adminService, IBasicCRUDService<Customer> customerService)
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
