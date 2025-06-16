using FribergCarRentals.Data;
using FribergCarRentals.Interfaces;
using FribergCarRentals.Models;

namespace FribergCarRentals.Services
{
    public class ReservationService : BasicCRUDService<Reservation>, IReservationService
    {
        public ReservationService(IRepository<Reservation> reservationRepository) : base(reservationRepository)
        {
        }
    }
}
