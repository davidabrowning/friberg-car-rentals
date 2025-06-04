using FribergCarRentals.Models;

namespace FribergCarRentals.Data
{
    public class CarRepository : IRepository<Car>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public CarRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void Add(Car car)
        {
            _applicationDbContext.Cars.Add(car);
            _applicationDbContext.SaveChanges();
        }

        public IEnumerable<Car> GetAll()
        {
            return _applicationDbContext.Cars.ToList();
        }

        public Car GetById(int id)
        {
            return _applicationDbContext.Cars.Where(c => c.Id == id).FirstOrDefault();
        }

        public bool IdExists(int id)
        {
            return _applicationDbContext.Cars.Any(c => c.Id == id);
        }

        public void Delete(int id)
        {
            Car car = GetById(id);
            _applicationDbContext.Remove(car);
            _applicationDbContext.SaveChanges();
        }

        public void Update(Car car)
        {
            _applicationDbContext.Update(car);
            _applicationDbContext.SaveChanges();
        }
    }
}
