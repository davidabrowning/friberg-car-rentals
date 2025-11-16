using FribergCarRentals.Core.Interfaces.Repositories;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;
using System.Threading.Tasks;

namespace FribergCarRentals.WebApi.Services
{
    public class CarServiceSeparated : BasicCRUDServiceSeparated<Car>, ICarService
    {
        private readonly IReservationService _reservationService;
        public CarServiceSeparated(IRepository<Car> carRepository, IReservationService reservationService) : base(carRepository)
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
