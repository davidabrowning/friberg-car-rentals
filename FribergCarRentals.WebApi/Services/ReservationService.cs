using FribergCarRentals.Core.Interfaces.Repositories;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;

namespace FribergCarRentals.WebApi.Services
{
    public class ReservationService : BasicCRUDService<Reservation>, IReservationService
    {
        public ReservationService(IRepository<Reservation> reservationRepository) : base(reservationRepository)
        {
        }

        public async Task<IEnumerable<Reservation>> GetByCustomerAsync(Customer customer)
        {
            IEnumerable<Reservation> allReservations = await GetAllAsync();
            IEnumerable<Reservation> myReservations = allReservations.Where(r => r.Customer == customer);
            return myReservations;
        }

        public async Task<IEnumerable<Reservation>> GetByCarAsync(Car car)
        {
            IEnumerable<Reservation> allReservations = await GetAllAsync();
            IEnumerable<Reservation> myReservations = allReservations.Where(r => r.Car == car);
            return myReservations;
        }

        public async Task<IEnumerable<Reservation>> DeleteByCarAsync(Car car)
        {
            IEnumerable<Reservation> reservations = await GetByCarAsync(car);
            foreach (Reservation reservation in reservations)
            {
                await DeleteAsync(reservation.Id);
            }
            return reservations;
        }

        public async Task<IEnumerable<Reservation>> DeleteByCustomerAsync(Customer customer)
        {
            IEnumerable<Reservation> reservations = await GetByCustomerAsync(customer);
            foreach (Reservation reservation in reservations)
            {
                await DeleteAsync(reservation.Id);
            }
            return reservations;
        }
    }
}
