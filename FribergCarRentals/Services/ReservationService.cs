using FribergCarRentals.Interfaces;
using FribergCarRentals.Models;

namespace FribergCarRentals.Services
{
    public class ReservationService : BasicCRUDService<Reservation>, IReservationService
    {
        private readonly IRepository<Reservation> _reservationRepository;
        public ReservationService(IRepository<Reservation> reservationRepository) : base(reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<IEnumerable<Reservation>> GetByCustomerAsync(Customer customer)
        {
            IEnumerable<Reservation> allReservations = await _reservationRepository.GetAllAsync();
            IEnumerable<Reservation> myReservations = allReservations.Where(r => r.Customer == customer);
            return myReservations;
        }
    }
}
