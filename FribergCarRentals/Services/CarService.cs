using FribergCarRentals.Interfaces;
using FribergCarRentals.Core.Models;
using System.Threading.Tasks;

namespace FribergCarRentals.Services
{
    public class CarService : BasicCRUDService<Car>, ICarService
    {
        private readonly IReservationService _reservationService;
        public CarService(IRepository<Car> carRepository, IReservationService reservationService) : base(carRepository)
        {
            _reservationService = reservationService;
        }

        public async override Task<Car?> DeleteAsync(int id)
        {
            Car? car = await GetByIdAsync(id);
            if (car == null)
            {
                return null;
            }

            IEnumerable<Reservation> reservations = await _reservationService.GetByCarAsync(car);
            foreach (Reservation reservation in reservations)
            {
                await _reservationService.DeleteAsync(reservation.Id);
            }

            return await base.DeleteAsync(id);
        }
    }
}
