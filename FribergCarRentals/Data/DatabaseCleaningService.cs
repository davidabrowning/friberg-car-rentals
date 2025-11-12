using FribergCarRentals.Interfaces;
using FribergCarRentals.Models;

namespace FribergCarRentals.Data
{
    public class DatabaseCleaningService
    {
        private readonly IAdminService _adminService;
        private readonly ICustomerService _customerService;
        public DatabaseCleaningService(IAdminService adminService, ICustomerService customerService)
        {
            _adminService = adminService;
            _customerService = customerService;
        }
        public async Task Go()
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
