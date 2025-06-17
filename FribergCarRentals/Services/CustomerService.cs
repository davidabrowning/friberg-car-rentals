using FribergCarRentals.Interfaces;
using FribergCarRentals.Models;

namespace FribergCarRentals.Services
{
    public class CustomerService : BasicCRUDService<Customer>, ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;
        public CustomerService(IRepository<Customer> customerRepository) : base(customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer?> DeleteCustomerByIdentityUserIdAsync(string identityUserId)
        {
            IEnumerable<Customer> customers = await _customerRepository.GetAllAsync();
            Customer? customer = customers.Where(c => c.IdentityUser.Id == identityUserId).FirstOrDefault();
            if (customer != null)
            {
                await _customerRepository.DeleteAsync(customer.Id);
            }
            return customer;
        }
    }
}
