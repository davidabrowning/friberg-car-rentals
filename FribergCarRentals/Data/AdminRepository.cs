using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentals.Data
{
    public class AdminRepository : IRepository<Admin>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        public AdminRepository(ApplicationDbContext applicationDbContext, UserManager<IdentityUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }
        public async Task Add(Admin admin)
        {
            _applicationDbContext.Admins.Add(admin);
            await _applicationDbContext.SaveChangesAsync();
            await AddToAdminRole(admin);
        }

        public async Task Delete(int id)
        {
            Admin? admin = await GetById(id);
            if (admin == null)
                return;
            _applicationDbContext.Remove(admin);
            await _applicationDbContext.SaveChangesAsync();
            await RemoveFromAdminRole(admin);
        }

        public async Task<IEnumerable<Admin>> GetAll()
        {
            return await _applicationDbContext.Admins.Include(a => a.IdentityUser).ToListAsync();
        }

        public async Task<Admin?> GetById(int id)
        {
            return await _applicationDbContext.Admins.Include(a => a.IdentityUser).Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> IdExists(int id)
        {
            return await _applicationDbContext.Admins.AnyAsync(a => a.Id == id);
        }

        public async Task Update(Admin admin)
        {
            _applicationDbContext.Update(admin);
            await _applicationDbContext.SaveChangesAsync();
        }

        // ========================================== Private helper methods ==========================================
        private async Task<IdentityUser?> GetIdentityUser(Admin admin)
        {
            if (admin.IdentityUser == null)
                return null;
            return await _userManager.FindByIdAsync(admin.IdentityUser.Id);
        }

        private async Task AddToAdminRole(Admin admin)
        {
            IdentityUser? identityUser = await GetIdentityUser(admin);
            if (identityUser == null)
                return;
            await _userManager.AddToRoleAsync(identityUser, "Admin");
        }

        private async Task RemoveFromAdminRole(Admin admin)
        {
            IdentityUser? identityUser = await GetIdentityUser(admin);
            if (identityUser == null)
                return;
            await _userManager.RemoveFromRoleAsync(identityUser, "Admin");
        }
    }
}
