using FribergCarRentals.Core.Constants;
using FribergCarRentals.Core.Interfaces.Repositories;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;

namespace FribergCarRentals.Services.Services
{
    public class CustomerService : BasicCrudService<Customer>, ICustomerService
    {
        public CustomerService(IRepository<Customer> customerRepository) : base(customerRepository)
        {
        }

        public async Task<Customer?> GetAsync(string userId)
        {
            IEnumerable<Customer> customers = await GetAllAsync();
            return customers.FirstOrDefault(a => a.UserId == userId);
        }
    }
}
