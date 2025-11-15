using FribergCarRentals.Core.Models;

namespace FribergCarRentals.Core.Interfaces
{
    public interface IAdminService : IBasicCRUDService<Admin>
    {
        Task<Admin?> DeleteAdminByIdentityUserIdAsync(string identityUserId);
    }
}
