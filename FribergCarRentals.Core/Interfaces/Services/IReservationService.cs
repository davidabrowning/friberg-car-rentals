using FribergCarRentals.Core.Models;

namespace FribergCarRentals.Core.Interfaces.Services
{
    public interface IReservationService : ICrudService<Reservation>
    {
        Task<IEnumerable<Reservation>> GetByCarAsync(Car car);
        Task<IEnumerable<Reservation>> GetByCustomerAsync(Customer customer);
    }
}
