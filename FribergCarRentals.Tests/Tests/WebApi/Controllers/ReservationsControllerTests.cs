using FribergCarRentals.Core.Models;
using FribergCarRentals.Tests.Mock.WebApi.Services;
using FribergCarRentals.WebApi.Controllers;
using FribergCarRentals.WebApi.Dtos;
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

        public ReservationsControllerTests() {
            MockReservationService mockReservationService = new();
            _reservationController = new(mockReservationService);
        }

        [Fact]
        public async Task GetAll_ReturnsCorrectType()
        {
            var result = await _reservationController.Get();
            Assert.IsType<List<ReservationDto>>(result);
        }

        [Fact]
        public async Task GetOne_ReturnsCorrectTypeAfterPost()
        {
            ReservationDto reservationDto = new();
            await _reservationController.Post(reservationDto);
            var result = await _reservationController.Get(reservationDto.Id);
            Assert.IsType<ReservationDto>(result);
        }

        [Fact]
        public async Task Post_ReturnsCorrectType()
        {
            ReservationDto reservationDto = new();
            var result = await _reservationController.Post(reservationDto);
            Assert.IsType<ReservationDto>(result);
        }

        [Fact]
        public async Task Put_UpdatesCarValue()
        {
            ReservationDto reservationDto = new();
            CarDto carDto = new() { Id = 42 };
            await _reservationController.Post(reservationDto);
            reservationDto.CarDto = carDto;
            await _reservationController.Put(reservationDto.Id, reservationDto);
            ReservationDto? result = await _reservationController.Get(reservationDto.Id);
            Assert.Equal(carDto.Id, result.CarDto.Id);
        }

        [Fact]
        public async Task Delete_RemovesReservation()
        {
            ReservationDto reservationDtoA = new() { Id = 7 };
            ReservationDto reservationDtoB = new() { Id = 8 };
            await _reservationController.Post(reservationDtoA);
            await _reservationController.Post(reservationDtoB);
            await _reservationController.Delete(reservationDtoA.Id);
            IEnumerable<ReservationDto> result = await _reservationController.Get();
            result = await _reservationController.Get();
            Assert.Single(result);
        }
    }
}
