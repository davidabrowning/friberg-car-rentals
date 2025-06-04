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
            try
            {
                await _applicationDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            IdentityUser identityUser = await _userManager.Users.Where(u => u.Id == admin.IdentityUser.Id).FirstOrDefaultAsync();
            var result = await _userManager.AddToRoleAsync(identityUser, "Admin");
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine("ERROR: " + error.Description);
                }
            }
        }

        public async Task Delete(int id)
        {
            Admin? admin = await GetById(id);
            if (admin == null)
                return;
            _applicationDbContext.Remove(admin);
            await _applicationDbContext.SaveChangesAsync();
            await _userManager.RemoveFromRoleAsync(admin.IdentityUser, "Admin");
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
    }
}
