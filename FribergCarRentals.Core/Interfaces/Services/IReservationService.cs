using FribergCarRentals.Core.Models;

namespace FribergCarRentals.Core.Interfaces.Services
{
    public interface IReservationService : IBasicCRUDService<Reservation>
    {
        Task<IEnumerable<Reservation>> GetByCarAsync(Car car);
        Task<IEnumerable<Reservation>> DeleteByCarAsync(Car car);
        Task<IEnumerable<Reservation>> GetByCustomerAsync(Customer customer);
        Task<IEnumerable<Reservation>> DeleteByCustomerAsync(Customer customer);
    }
}
