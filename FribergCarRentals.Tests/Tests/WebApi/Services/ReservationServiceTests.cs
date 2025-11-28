using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;
using FribergCarRentals.Tests.Mock.Data.Repositories;
using FribergCarRentals.WebApi.Services;

namespace FribergCarRentals.Tests.Tests.WebApi.Services
{
    public class ReservationServiceTests
    {
        private IReservationService _reservationService;

        public ReservationServiceTests()
        {
            MockReservationRepository reservationRepository = new();
            _reservationService = new ReservationService(reservationRepository);
        }

        [Fact]
        public async Task GetByCarAsync_ShouldReturnEmptyListIfNewCar()
        {
            Car car = new();
            IEnumerable<Reservation> reservations = await _reservationService.GetByCarAsync(car);
            Assert.Empty(reservations);
        }

        [Fact]
        public async Task GetByCarAsync_ShouldReturnListOfOneIfCarHasBeenAdded()
        {
            Car car = new();
            Reservation reservation = new() { Car = car };
            await _reservationService.CreateAsync(reservation);
            IEnumerable<Reservation> reservations = await _reservationService.GetByCarAsync(car);
            Assert.Single(reservations);
        }

        [Fact]
        public async Task GetByCarAsync_ShouldReturnEmptyIfCarIsNotAdded()
        {
            Car car = new();
            Reservation reservation = new() { Car = car };
            IEnumerable<Reservation> reservations = await _reservationService.GetByCarAsync(car);
            Assert.Empty(reservations);
        }
    }
}
