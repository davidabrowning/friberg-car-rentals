using FribergCarRentals.Core.Interfaces.Repositories;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;

namespace FribergCarRentals.WebApi.Services
{
    public class CarService : BasicCrudService<Car>, ICarService
    {
        public CarService(IRepository<Car> carRepository) : base(carRepository)
        {
        }
    }
}
