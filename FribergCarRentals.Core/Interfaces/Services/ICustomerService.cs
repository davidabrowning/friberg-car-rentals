using FribergCarRentals.Core.Models;

namespace FribergCarRentals.Core.Interfaces.Services
{
    public interface ICustomerService : IBasicCRUDService<Customer>
    {
        Task<Customer?> DeleteCustomerByIdentityUserIdAsync(string identityUserId);
    }
}
