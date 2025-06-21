using FribergCarRentals.Interfaces;
using FribergCarRentals.Models;

namespace FribergCarRentals.Services
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
    }
}
