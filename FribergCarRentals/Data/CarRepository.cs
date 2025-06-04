using FribergCarRentals.Models;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentals.Data
{
    public class CarRepository : IRepository<Car>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public CarRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task Add(Car car)
        {
            await _applicationDbContext.Cars.AddAsync(car);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Car>> GetAll()
        {
            return await _applicationDbContext.Cars.ToListAsync();
        }

        public async Task<Car?> GetById(int id)
        {
            return await _applicationDbContext.Cars.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> IdExists(int id)
        {
            return await _applicationDbContext.Cars.AnyAsync(c => c.Id == id);
        }

        public async Task Delete(int id)
        {
            Car? car = await GetById(id);
            if (car == null)
                return;
            _applicationDbContext.Remove(car);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task Update(Car car)
        {
            _applicationDbContext.Update(car);
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
