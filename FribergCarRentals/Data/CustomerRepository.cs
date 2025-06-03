using FribergCarRentals.Models;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentals.Data
{
    public class CustomerRepository : ICustomer
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public CustomerRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void Add(Customer customer)
        {
            _applicationDbContext.Customers.Add(customer);
            _applicationDbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            Customer customer = GetById(id);
            _applicationDbContext.Remove(customer);
            _applicationDbContext.SaveChanges();
        }

        public IEnumerable<Customer> GetAll()
        {
            return _applicationDbContext.Customers.Include(c => c.IdentityUser).ToList();
        }

        public Customer GetById(int id)
        {
            return _applicationDbContext.Customers.Where(c => c.Id == id).Include(c => c.IdentityUser).FirstOrDefault();
        }

        public bool IdExists(int id)
        {
            return _applicationDbContext.Customers.Where(c => c.Id == id).Any();
        }

        public void Update(Customer customer)
        {
            _applicationDbContext.Update(customer);
            _applicationDbContext.SaveChanges();
        }
    }
}
