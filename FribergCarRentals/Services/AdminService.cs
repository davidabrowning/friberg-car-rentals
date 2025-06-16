using FribergCarRentals.Data;
using FribergCarRentals.Interfaces;
using FribergCarRentals.Models;

namespace FribergCarRentals.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepository<Admin> _adminRepository;
        public AdminService(IRepository<Admin> adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<Admin> AddAsync(Admin admin)
        {
            await _adminRepository.AddAsync(admin);
            return admin;
        }

        public async Task<Admin?> DeleteAsync(int id)
        {
            Admin? admin = await GetByIdAsync(id);
            if (admin == null)
            {
                return null;
            }

            await _adminRepository.DeleteAsync(id);

            return admin;
        }

        public async Task<Admin?> GetByIdAsync(int id)
        {
            return await _adminRepository.GetByIdAsync(id);
        }

        public async Task<bool> IdExistsAsync(int id)
        {
            return await _adminRepository.IdExistsAsync(id);
        }

        public async Task<Admin> UpdateAsync(Admin admin)
        {
            await _adminRepository.UpdateAsync(admin);
            return admin;
        }
    }
}
