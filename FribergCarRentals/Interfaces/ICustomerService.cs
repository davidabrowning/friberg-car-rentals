using FribergCarRentals.Models;

namespace FribergCarRentals.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer?> GetByIdAsync(int id);
        Task<Customer> AddAsync(Customer customer);
        Task<Customer> UpdateAsync(Customer customer);
        Task<Customer?> DeleteAsync(int id);
        Task<bool> IdExistsAsync(int id);
    }
}
