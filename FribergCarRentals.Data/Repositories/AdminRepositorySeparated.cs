using FribergCarRentals.Data;
using FribergCarRentals.Core.Models;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.Core.Interfaces.Repositories;

namespace FribergCarRentals.Data
{
    public class AdminRepositorySeparated : IRepository<Admin>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public AdminRepositorySeparated(ApplicationDbContext applicationDbContext)
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
