using FribergCarRentals.Models;

namespace FribergCarRentals.Interfaces
{
    public interface ICustomerService : IBasicCRUDService<Customer>
    {
        Task<Customer?> DeleteCustomerByIdentityUserIdAsync(string identityUserId);
    }
}
