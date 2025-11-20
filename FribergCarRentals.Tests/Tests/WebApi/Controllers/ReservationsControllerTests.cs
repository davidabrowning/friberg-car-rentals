using FribergCarRentals.Core.Models;
using FribergCarRentals.Tests.Mock.WebApi.Services;
using FribergCarRentals.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.Tests.Tests.WebApi.Controllers
{
    public class ReservationsControllerTests
    {
        private ReservationsController _reservationController;
        private Reservation _reservation;
        public ReservationsControllerTests() {
            MockReservationService mockReservationService = new();
            _reservationController = new(mockReservationService);
            _reservation = new() { Id = 42 };
        }

        [Fact]
        public async Task GetAll_ReturnsCorrectType()
        {
            var result = await _reservationController.Get();
            Assert.IsType<List<Reservation>>(result);
        }

        [Fact]
        public async Task GetOne_ReturnsCorrectTypeAfterPost()
        {
            await _reservationController.Post(_reservation);
            var result = await _reservationController.Get(_reservation.Id);
            Assert.IsType<Reservation>(result);
        }

        [Fact]
        public async Task Post_ReturnsCorrectType()
        {
            var result = await _reservationController.Post(_reservation);
            Assert.IsType<Reservation>(result);
        }

        [Fact]
        public async Task Put_UpdatesCarValue()
        {
            Car car = new Car() { Id = 88 };
            await _reservationController.Post(_reservation);
            _reservation.Car = car;
            await _reservationController.Put(_reservation.Id, _reservation);
            Reservation? result = await _reservationController.Get(_reservation.Id);
            Assert.Equal(car, result.Car);
        }

        [Fact]
        public async Task Delete_RemovesReservation()
        {
            await _reservationController.Post(new Reservation() { Id = 5 });
            await _reservationController.Post(new Reservation() { Id = 7 });
            await _reservationController.Delete(5);
            IEnumerable<Reservation> result = await _reservationController.Get();
            result = await _reservationController.Get();
            Assert.Single(result);
        }
    }
}
