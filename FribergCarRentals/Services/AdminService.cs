using FribergCarRentals.Interfaces;
using FribergCarRentals.Models;

namespace FribergCarRentals.Services
{
    public class AdminService : BasicCRUDService<Admin>, IAdminService
    {
        private readonly IRepository<Admin> _adminRepository;
        public AdminService(IRepository<Admin> adminRepository) : base(adminRepository)
        {
            _adminRepository = adminRepository;
        }
        public async Task<Admin?> DeleteAdminByIdentityUserIdAsync(string identityUserId)
        {
            IEnumerable<Admin> admins = await _adminRepository.GetAllAsync();
            Admin? admin = admins.Where(a => a.IdentityUser.Id == identityUserId).FirstOrDefault();
            if (admin != null)
            {
                await _adminRepository.DeleteAsync(admin.Id);
            }
            return admin;
        }
    }
}
