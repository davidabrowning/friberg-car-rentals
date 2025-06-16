using FribergCarRentals.Interfaces;
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

        public async Task AddAsync(Car car)
        {
            await _applicationDbContext.Cars.AddAsync(car);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Car>> GetAllAsync()
        {
            return await _applicationDbContext.Cars.ToListAsync();
        }

        public async Task<Car?> GetByIdAsync(int id)
        {
            return await _applicationDbContext.Cars.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> IdExistsAsync(int id)
        {
            return await _applicationDbContext.Cars.AnyAsync(c => c.Id == id);
        }

        public async Task DeleteAsync(int id)
        {
            Car? car = await GetByIdAsync(id);
            if (car == null)
                return;
            _applicationDbContext.Remove(car);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Car car)
        {
            _applicationDbContext.Update(car);
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
