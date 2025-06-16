using FribergCarRentals.Interfaces;
using FribergCarRentals.Models;

namespace FribergCarRentals.Data
{
    public class ReservationRepository : IRepository<Reservation>
    {
        public Task AddAsync(Reservation t)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reservation>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Reservation?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IdExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Reservation t)
        {
            throw new NotImplementedException();
        }
    }
}
