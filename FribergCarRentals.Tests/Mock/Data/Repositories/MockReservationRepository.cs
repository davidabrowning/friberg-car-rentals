using FribergCarRentals.Core.Interfaces.Repositories;
using FribergCarRentals.Core.Models;

namespace FribergCarRentals.Tests.Mock.Data.Repositories
{
    public class MockReservationRepository : IRepository<Reservation>
    {
        private List<Reservation> _reservations = new();
        public Task AddAsync(Reservation reservation)
        {
            _reservations.Add(reservation);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reservation>> GetAllAsync()
        {
            IEnumerable<Reservation> reservations = _reservations;
            return Task.FromResult(reservations);
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
