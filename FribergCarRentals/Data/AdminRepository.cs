using FribergCarRentals.Models;

namespace FribergCarRentals.Data
{
    public class AdminRepository : IRepository<Admin>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public AdminRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public void Add(Admin admin)
        {
            _applicationDbContext.Admins.Add(admin);
            _applicationDbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            Admin admin = _applicationDbContext.Admins.Where(a  => a.Id == id).FirstOrDefault();
            _applicationDbContext.Remove(admin);
            _applicationDbContext.SaveChanges();
        }

        public IEnumerable<Admin> GetAll()
        {
            return _applicationDbContext.Admins.ToList();
        }

        public Admin GetById(int id)
        {
            return _applicationDbContext.Admins.Where(a => a.Id == id).FirstOrDefault();
        }

        public bool IdExists(int id)
        {
            return _applicationDbContext.Admins.Any(a => a.Id == id);
        }

        public void Update(Admin admin)
        {
            _applicationDbContext.Update(admin);
            _applicationDbContext.SaveChanges();
        }
    }
}
