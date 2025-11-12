using FribergCarRentals.Interfaces;
using FribergCarRentals.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentals.Data
{
    public class AdminRepository : IRepository<Admin>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public AdminRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task AddAsync(Admin admin)
        {
            _applicationDbContext.Admins.Add(admin);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Admin? admin = await GetByIdAsync(id);
            if (admin == null)
                return;
            _applicationDbContext.Remove(admin);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Admin>> GetAllAsync()
        {
            return await _applicationDbContext.Admins.ToListAsync();
        }

        public async Task<Admin?> GetByIdAsync(int id)
        {
            return await _applicationDbContext.Admins.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> IdExistsAsync(int id)
        {
            return await _applicationDbContext.Admins.AnyAsync(a => a.Id == id);
        }

        public async Task UpdateAsync(Admin admin)
        {
            _applicationDbContext.Update(admin);
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
