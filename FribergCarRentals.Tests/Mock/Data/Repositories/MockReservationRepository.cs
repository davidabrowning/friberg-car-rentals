using FribergCarRentals.Core.Interfaces.Repositories;
using FribergCarRentals.Core.Models;
using FribergCarRentals.Tests.Mock.Data.Database;

namespace FribergCarRentals.Tests.Mock.Data.Repositories
{
    public class MockReservationRepository : IRepository<Reservation>
    {
        public Task AddAsync(Reservation reservation)
        {
            MockDatabase.Reservations.Add(reservation);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reservation>> GetAllAsync()
        {
            IEnumerable<Reservation> reservations = MockDatabase.Reservations;
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
