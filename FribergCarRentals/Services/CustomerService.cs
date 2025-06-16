using FribergCarRentals.Data;
using FribergCarRentals.Interfaces;
using FribergCarRentals.Models;

namespace FribergCarRentals.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;
        public CustomerService(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<Customer> AddAsync(Customer customer)
        {
            await _customerRepository.AddAsync(customer);
            return customer;
        }

        public async Task<Customer?> DeleteAsync(int id)
        {
            Customer? customer = await GetByIdAsync(id);
            if (customer == null)
            {
                return null;
            }

            await _customerRepository.DeleteAsync(id);
            return customer;
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        public async Task<bool> IdExistsAsync(int id)
        {
            return await _customerRepository.IdExistsAsync(id);
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            await _customerRepository.UpdateAsync(customer);
            return customer;
        }
    }
}
