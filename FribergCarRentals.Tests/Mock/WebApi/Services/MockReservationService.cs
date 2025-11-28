using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;

namespace FribergCarRentals.Tests.Mock.WebApi.Services
{
    public class MockReservationService : IReservationService
    {
        private List<Reservation> _reservations = new List<Reservation>();
        public Task<Reservation> CreateAsync(Reservation reservation)
        {
            _reservations.Add(reservation);
            return Task.FromResult(reservation);
        }

        public async Task<Reservation?> DeleteAsync(int id)
        {
            Reservation? reservation = await GetAsync(id);
            if (reservation == null) {
                return null;
            }
            _reservations.Remove(reservation);
            return reservation;
        }

        public Task<IEnumerable<Reservation>> DeleteByCarAsync(Car car)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reservation>> DeleteByCustomerAsync(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reservation>> GetAllAsync()
        {
            IEnumerable<Reservation> result = _reservations.ToList();
            return Task.FromResult(result);
        }

        public Task<IEnumerable<Reservation>> GetByCarAsync(Car car)
        {
            IEnumerable<Reservation> result = _reservations.Where(r => r.Car == car).ToList();
            return Task.FromResult(result);
        }

        public Task<IEnumerable<Reservation>> GetByCustomerAsync(Customer customer)
        {
            IEnumerable<Reservation> result = _reservations.Where(r => r.Customer == customer).ToList();
            return Task.FromResult(result);
        }

        public Task<Reservation?> GetAsync(int id)
        {
            Reservation? result = _reservations.Where(r => r.Id == id).FirstOrDefault();
            return Task.FromResult(result);
        }

        public Task<bool> IdExistsAsync(int id)
        {
            bool result = GetAsync(id) != null;
            return Task.FromResult(result);
        }

        public async Task<Reservation> UpdateAsync(Reservation reservation)
        {
            Reservation? oldReservation = await GetAsync(reservation.Id);
            _reservations.Remove(oldReservation);
            _reservations.Add(reservation);
            return reservation;
        }
    }
}
