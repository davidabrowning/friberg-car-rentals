using FribergCarRentals.Models;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentals.Data
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public CustomerRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task Add(Customer customer)
        {
            _applicationDbContext.Customers.Add(customer);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Customer? customer = await GetById(id);
            if (customer == null)
                return;
            _applicationDbContext.Remove(customer);
            await _applicationDbContext.SaveChangesAsync();
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
    }
}
