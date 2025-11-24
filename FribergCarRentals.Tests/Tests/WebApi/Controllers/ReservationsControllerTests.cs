using FribergCarRentals.Core.Models;
using FribergCarRentals.Tests.Mock.WebApi.Services;
using FribergCarRentals.WebApi.Controllers;
using FribergCarRentals.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;
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
    }
}
