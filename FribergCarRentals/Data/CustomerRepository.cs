using FribergCarRentals.Interfaces;
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

        public async Task AddAsync(Customer customer)
        {
            _applicationDbContext.Customers.Add(customer);
            await _applicationDbContext.SaveChangesAsync();
            await AddToCustomerRole(customer);
        }

        public async Task DeleteAsync(int id)
        {
            Customer? customer = await GetByIdAsync(id);
            if (customer == null)
                return;
            _applicationDbContext.Remove(customer);
            await _applicationDbContext.SaveChangesAsync();
            await RemoveFromCustomerRole(customer);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _applicationDbContext.Customers
                .Include(c => c.Reservations)
                    .ThenInclude(r => r.Car)
                .ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _applicationDbContext.Customers
                .Where(c => c.Id == id)
                .Include(c => c.Reservations)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IdExistsAsync(int id)
        {
            return await _applicationDbContext.Customers.Where(c => c.Id == id).AnyAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            _applicationDbContext.Update(customer);
            await _applicationDbContext.SaveChangesAsync();
        }

        // ========================================== Private helper methods ==========================================
        private async Task<IdentityUser?> GetIdentityUser(Customer customer)
        {
            if (customer.UserId == null)
                return null;
            return await _userManager.FindByIdAsync(customer.UserId);
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
