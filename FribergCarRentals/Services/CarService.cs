using FribergCarRentals.Interfaces;
using FribergCarRentals.Models;

namespace FribergCarRentals.Services
{
    public class CarService : BasicCRUDService<Car>, ICarService
    {
        public CarService(IRepository<Car> carRepository) : base(carRepository)
        {
        }
    }
}
