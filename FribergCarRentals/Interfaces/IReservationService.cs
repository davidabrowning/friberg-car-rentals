using FribergCarRentals.Models;

namespace FribergCarRentals.Interfaces
{
    public interface IReservationService : IBasicCRUDService<Reservation>
    {
        Task<IEnumerable<Reservation>> GetByCustomerAsync(Customer customer);
    }
}
