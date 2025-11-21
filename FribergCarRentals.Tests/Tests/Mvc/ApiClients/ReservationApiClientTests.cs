using FribergCarRentals.Core.Models;
using FribergCarRentals.Mvc.ApiClients;
using FribergCarRentals.Tests.Mock.Other;
using FribergCarRentals.WebApi.Dtos;
using FribergCarRentals.WebApi.Mappers;

namespace FribergCarRentals.Tests.Tests.Mvc.ApiClients
{
    public class ReservationApiClientTests
    {
        private MockHttpMessageHandler _mockHttpMessageHandler;
        private ReservationApiClient _reservationApiClient;

        public ReservationApiClientTests()
        {
            _mockHttpMessageHandler = new();
            HttpClient httpClient = new(_mockHttpMessageHandler) { BaseAddress = new Uri("http://localhost") };
            _reservationApiClient = new(httpClient);
        }

        [Fact]
        public async Task GetAll_IsConfiguredCorrectly()
        {
            _mockHttpMessageHandler.ResponseObject = new List<ReservationDto>();
            var result = await _reservationApiClient.GetAsync();
            Assert.IsType<List<ReservationDto>>(result);
            Assert.Equal("/api/reservations", _mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Get, _mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task GetOne_IsConfiguredCorrectly()
        {
            ReservationDto reservationDto = new();
            _mockHttpMessageHandler.ResponseObject = reservationDto;
            var result = await _reservationApiClient.GetAsync(reservationDto.Id);
            Assert.IsType<ReservationDto>(result);
            Assert.Equal($"/api/reservations/{reservationDto.Id}", _mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Get, _mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task Post_IsConfiguredCorrectly()
        {
            ReservationDto reservationDto = new();
            _mockHttpMessageHandler.ResponseObject = reservationDto;
            var result = await _reservationApiClient.PostAsync(reservationDto);
            Assert.IsType<ReservationDto>(result);
            Assert.Equal("/api/reservations", _mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Post, _mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task Put_IsConfiguredCorrectly()
        {
            ReservationDto reservationDto = new();
            _mockHttpMessageHandler.ResponseObject = new ReservationDto();
            var result = await _reservationApiClient.PutAsync(reservationDto);
            Assert.IsType<ReservationDto>(result);
            Assert.Equal($"/api/reservations/{reservationDto.Id}", _mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Put, _mockHttpMessageHandler.RequestMethod);
        }   

        [Fact]
        public async Task Delete_IsConfiguredCorrectly()
        {
            ReservationDto reservationDto = new();
            _mockHttpMessageHandler.ResponseObject = new ReservationDto();
            var result = await _reservationApiClient.DeleteAsync(reservationDto.Id);
            Assert.IsType<ReservationDto>(result);
            Assert.Equal($"/api/reservations/{reservationDto.Id}", _mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Delete, _mockHttpMessageHandler.RequestMethod);
        }
    }
}
