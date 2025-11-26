using FribergCarRentals.Core.Models;

namespace FribergCarRentals.Core.Interfaces.Services
{
    public interface ICustomerService : ICrudService<Customer>
    {
        Task<Customer?> DeleteCustomerByIdentityUserIdAsync(string identityUserId);
    }
}
