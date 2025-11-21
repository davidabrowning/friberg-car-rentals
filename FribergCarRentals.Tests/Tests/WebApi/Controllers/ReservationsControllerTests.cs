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
        private Reservation _reservation;
        private ReservationDto _reservationDto;
        private CarDto _carDto;
        public ReservationsControllerTests() {
            MockReservationService mockReservationService = new();
            _reservationController = new(mockReservationService);
            _reservation = new() { Id = 42 };
            _reservationDto = new()
            {
                Id = 43,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now),
                CarDto = new()
                {
                    Id = 0,
                    Make = "",
                    Model = "",
                    Year = 0,
                    Description = "",
                    PhotoUrls = new()
                },
                CustomerDto = new()
                {
                    Id = 0,
                    UserId = "",
                    FirstName = "",
                    LastName = "",
                    HomeCity = "",
                    HomeCountry = ""
                }
            };
            _carDto = new()
            {
                Id = 1,
                Make = "",
                Model = "",
                Description = "",
                Year = 0,
                PhotoUrls = new()
            };
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
            await _reservationController.Post(_reservationDto);
            var result = await _reservationController.Get(_reservationDto.Id);
            Assert.IsType<ReservationDto>(result);
        }

        [Fact]
        public async Task Post_ReturnsCorrectType()
        {
            var result = await _reservationController.Post(_reservationDto);
            Assert.IsType<ReservationDto>(result);
        }

        [Fact]
        public async Task Put_UpdatesCarValue()
        {
            await _reservationController.Post(_reservationDto);
            _reservationDto.CarDto = _carDto;
            await _reservationController.Put(_reservation.Id, _reservationDto);
            ReservationDto? result = await _reservationController.Get(_reservationDto.Id);
            Assert.Equal(_carDto, result.CarDto);
        }

        [Fact]
        public async Task Delete_RemovesReservation()
        {
            await _reservationController.Post(_reservationDto);
            await _reservationController.Delete(_reservationDto.Id);
            IEnumerable<ReservationDto> result = await _reservationController.Get();
            result = await _reservationController.Get();
            Assert.Empty(result);
        }
    }
}
