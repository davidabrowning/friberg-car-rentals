using FribergCarRentals.Core.Interfaces.Repositories;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;

namespace FribergCarRentals.WebApi.Services
{
    public class AdminService : BasicCrudService<Admin>, IAdminService
    {
        public AdminService(IRepository<Admin> adminRepository) : base(adminRepository)
        {
        }

        public async Task<Admin?> GetAsync(string userId)
        {
            IEnumerable<Admin> admins = await GetAllAsync();
            return admins.FirstOrDefault(a => a.UserId == userId);
        }
    }
}
