using FribergCarRentals.Core.Interfaces.Repositories;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;

namespace FribergCarRentals.WebApi.Services
{
    public class ReservationServiceSeparated : BasicCRUDServiceSeparated<Reservation>, IReservationService
    {
        public ReservationServiceSeparated(IRepository<Reservation> reservationRepository) : base(reservationRepository)
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
