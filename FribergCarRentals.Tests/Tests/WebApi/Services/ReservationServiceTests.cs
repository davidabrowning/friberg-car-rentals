using FribergCarRentals.Core.Interfaces.Repositories;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;
using FribergCarRentals.Tests.Mock.Data.Repositories;
using FribergCarRentals.WebApi.Services;
using Microsoft.IdentityModel.Tokens;

namespace FribergCarRentals.Tests.Tests.WebApi.Services
{
    public class ReservationServiceTests
    {
        // Reused variables
        private IReservationService reservationService;
        private IEnumerable<Reservation>? reservations;
        private Car? car;
        private Reservation? reservation;

        public ReservationServiceTests()
        {
            IRepository<Reservation> reservationRepository = new MockReservationRepository();
            reservationService = new ReservationServiceSeparated(reservationRepository);
        }

        [Fact]
        public async Task GetByCarAsync_ShouldReturnEmptyListIfNewCar()
        {
            car = new();
            reservations = await reservationService.GetByCarAsync(car);
            Assert.Empty(reservations);
        }

        [Fact]
        public async Task GetByCarAsync_ShouldReturnListOfOneIfCarHasBeenAdded()
        {
            car = new();
            reservation = new() { Car = car };
            await reservationService.CreateAsync(reservation);
            reservations = await reservationService.GetByCarAsync(car);
            Assert.Single(reservations);
        }

        [Fact]
        public async Task GetByCarAsync_ShouldReturnEmptyIfTestedAgainWithoutReaddingCar()
        {
            car = new();
            reservation = new() { Car = car };
            // await reservationService.CreateAsync(reservation); <== tests that this line is required
            //                                                        to re-add reservation
            reservations = await reservationService.GetByCarAsync(car);
            Assert.Empty(reservations);
        }
    }
}
