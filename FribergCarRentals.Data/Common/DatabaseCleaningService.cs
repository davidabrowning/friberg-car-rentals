using FribergCarRentals.Core.Interfaces.Facades;
using FribergCarRentals.Core.Interfaces.Other;
using FribergCarRentals.Core.Models;

namespace FribergCarRentals.Data
{
    public class DatabaseCleaningService : IDatabaseCleaner
    {
        IApplicationFacade _applicationFacade;
        public DatabaseCleaningService(IApplicationFacade applicationFacade)
        {
            _applicationFacade = applicationFacade;
        }
        public async Task CleanAsync()
        {
            await RemoveAdminsWithoutIdentityUsers();
            await RemoveCustomersWithoutIdentityUsers();
        }
        private async Task RemoveAdminsWithoutIdentityUsers()
        {
            IEnumerable<Admin> admins = await _applicationFacade.GetAllAdminsAsync();
            foreach (Admin admin in admins)
            {
                if (admin.UserId == null)
                {
                    await _applicationFacade.DeleteAdminAsync(admin.Id);
                }
            }
        }
        private async Task RemoveCustomersWithoutIdentityUsers()
        {
            IEnumerable<Customer> customers = await _applicationFacade.GetAllCustomersAsync();
            foreach (Customer customer in customers)
            {
                if (customer.UserId == null)
                {
                    await _applicationFacade.DeleteCustomerAsync(customer.Id);
                }
            }
        }
    }
}
