using FribergCarRentals.Interfaces;
using FribergCarRentals.Models;

namespace FribergCarRentals.Services
{
    public class CustomerService : BasicCRUDService<Customer>, ICustomerService
    {
        public CustomerService(IRepository<Customer> customerRepository) : base(customerRepository)
        {
        }
    }
}
