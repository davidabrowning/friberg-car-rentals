using FribergCarRentals.Models;

namespace FribergCarRentals.Data
{
    public class DatabaseCleaningService
    {
        private readonly IRepository<Admin> _adminRepository;
        private readonly IRepository<Customer> _customerRepository;
        public DatabaseCleaningService(IRepository<Admin> adminRepository, IRepository<Customer> customerRepository)
        {
            _adminRepository = adminRepository;
            _customerRepository = customerRepository;
        }
        public async Task Go()
        {
            await RemoveAdminsWithoutIdentityUsers();
            await RemoveCustomersWithoutIdentityUsers();
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
    }
}
