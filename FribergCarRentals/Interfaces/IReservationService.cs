using FribergCarRentals.Core.Models;

namespace FribergCarRentals.Interfaces
{
    public interface IReservationService : IBasicCRUDService<Reservation>
    {
        Task<IEnumerable<Reservation>> GetByCarAsync(Car car);
        Task<IEnumerable<Reservation>> GetByCustomerAsync(Customer customer);
    }
}
