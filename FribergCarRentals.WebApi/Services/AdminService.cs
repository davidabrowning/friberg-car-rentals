using FribergCarRentals.Core.Interfaces.Repositories;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;

namespace FribergCarRentals.WebApi.Services
{
    public class AdminService : BasicCRUDService<Admin>, IAdminService
    {
        public AdminService(IRepository<Admin> adminRepository) : base(adminRepository)
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
