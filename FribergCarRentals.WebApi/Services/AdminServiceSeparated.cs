using FribergCarRentals.Core.Interfaces;
using FribergCarRentals.Core.Models;

namespace FribergCarRentals.WebApi.Services
{
    public class AdminServiceSeparated : BasicCRUDServiceSeparated<Admin>, IAdminService
    {
        public AdminServiceSeparated(IRepository<Admin> adminRepository) : base(adminRepository)
        {
        }
        public async Task<Admin?> DeleteAdminByIdentityUserIdAsync(string identityUserId)
        {
            IEnumerable<Admin> admins = await GetAllAsync();
            Admin? admin = admins.Where(a => a.UserId == identityUserId).FirstOrDefault();
            if (admin != null)
            {
                await DeleteAsync(admin.Id);
            }
            return admin;
        }
    }
}
