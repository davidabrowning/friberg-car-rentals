using FribergCarRentals.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace FribergCarRentals.Interfaces
{
    public interface ICustomerService : IBasicCRUDService<Customer>
    {
        Task<Customer?> DeleteCustomerByIdentityUserIdAsync(string identityUserId);
    }
}
