using FribergCarRentals.Core.Models;

namespace FribergCarRentals.Core.Interfaces.Services
{
    public interface IAdminService : IBasicCRUDService<Admin>
    {
        Task<Admin?> DeleteAdminByIdentityUserIdAsync(string identityUserId);
    }
}
