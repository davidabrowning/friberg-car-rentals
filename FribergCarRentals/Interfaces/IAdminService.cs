using FribergCarRentals.Models;

namespace FribergCarRentals.Interfaces
{
    public interface IAdminService : IBasicCRUDService<Admin>
    {
        Task<Admin?> DeleteAdminByIdentityUserIdAsync(string identityUserId);
    }
}
