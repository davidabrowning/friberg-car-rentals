using FribergCarRentals.Models;

namespace FribergCarRentals.Interfaces
{
    public interface IAdminService
    {
        Task<Admin> AddAsync(Admin admin);
        Task<Admin?> GetByIdAsync(int id);
        Task<Admin> UpdateAsync(Admin admin);
        Task<Admin?> DeleteAsync(int id);
        Task<bool> IdExistsAsync(int id);
    }
}
