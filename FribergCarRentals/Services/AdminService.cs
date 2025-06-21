using FribergCarRentals.Interfaces;
using FribergCarRentals.Models;

namespace FribergCarRentals.Services
{
    public class AdminService : BasicCRUDService<Admin>, IAdminService
    {
        public AdminService(IRepository<Admin> adminRepository) : base(adminRepository)
        {
        }
        public async Task<Admin?> DeleteAdminByIdentityUserIdAsync(string identityUserId)
        {
            IEnumerable<Admin> admins = await GetAllAsync();
            Admin? admin = admins.Where(a => a.IdentityUser.Id == identityUserId).FirstOrDefault();
            if (admin != null)
            {
                await DeleteAsync(admin.Id);
            }
            return admin;
        }
    }
}
