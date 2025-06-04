using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentals.Data
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        public CustomerRepository(ApplicationDbContext applicationDbContext, UserManager<IdentityUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }

        public async Task Add(Customer customer)
        {
            _applicationDbContext.Customers.Add(customer);
            await _applicationDbContext.SaveChangesAsync();
            await AddToCustomerRole(customer);
        }

        public async Task Delete(int id)
        {
            Customer? customer = await GetById(id);
            if (customer == null)
                return;
            _applicationDbContext.Remove(customer);
            await _applicationDbContext.SaveChangesAsync();
            await RemoveFromCustomerRole(customer);
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await _applicationDbContext.Customers.Include(c => c.IdentityUser).ToListAsync();
        }

        public async Task<Customer?> GetById(int id)
        {
            return await _applicationDbContext.Customers.Where(c => c.Id == id).Include(c => c.IdentityUser).FirstOrDefaultAsync();
        }

        public async Task<bool> IdExists(int id)
        {
            return await _applicationDbContext.Customers.Where(c => c.Id == id).AnyAsync();
        }

        public async Task Update(Customer customer)
        {
            _applicationDbContext.Update(customer);
            await _applicationDbContext.SaveChangesAsync();
        }

        // ========================================== Private helper methods ==========================================
        private async Task<IdentityUser?> GetIdentityUser(Customer customer)
        {
            if (customer.IdentityUser == null)
                return null;
            return await _userManager.FindByIdAsync(customer.IdentityUser.Id);
        }

        private async Task AddToCustomerRole(Customer customer)
        {
            IdentityUser? identityUser = await GetIdentityUser(customer);
            if (identityUser == null)
                return;
            await _userManager.AddToRoleAsync(identityUser, "Customer");
        }

        private async Task RemoveFromCustomerRole(Customer customer)
        {
            IdentityUser? identityUser = await GetIdentityUser(customer);
            if (identityUser == null)
                return;
            await _userManager.RemoveFromRoleAsync(identityUser, "Customer");
        }
    }
}
