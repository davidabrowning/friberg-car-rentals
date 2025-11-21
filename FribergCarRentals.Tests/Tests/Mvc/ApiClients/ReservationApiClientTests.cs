using FribergCarRentals.Core.Models;
using FribergCarRentals.Mvc.ApiClients;
using FribergCarRentals.Tests.Mock.Other;
using FribergCarRentals.WebApi.Dtos;
using FribergCarRentals.WebApi.Mappers;

namespace FribergCarRentals.Tests.Tests.Mvc.ApiClients
{
    public class ReservationApiClientTests
    {
        // Reused variables
        private MockHttpMessageHandler mockHttpMessageHandler;
        private ReservationApiClient reservationApiClient;
        private List<Reservation> reservations;
        private Reservation reservation;
        private ReservationDto reservationDto;
        private List<ReservationDto> reservationDtos;

        public ReservationApiClientTests()
        {
            mockHttpMessageHandler = new();
            HttpClient httpClient = new(mockHttpMessageHandler) { BaseAddress = new Uri("http://localhost") };
            reservationApiClient = new(httpClient);
            reservations = new() { new Reservation() { Id = 27 }, new Reservation() { Id = 28 } };
            reservation = new() { Id = 42 };
            reservationDto = new()
            {
                Id = 43,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now),
                CarDto = new() { 
                    Id = 0,
                    Make = "",
                    Model = "",
                    Year = 0,
                    Description = "",
                    PhotoUrls = new()
                },
                CustomerDto = new() { 
                    Id = 0,
                    UserId = "",
                    FirstName = "",
                    LastName = "",
                    HomeCity = "",
                    HomeCountry = ""
                }
            };
        }

        [Fact]
        public async Task GetAll_IsConfiguredCorrectly()
        {
            mockHttpMessageHandler.ResponseObject = reservationDtos;
            var result = await reservationApiClient.GetAsync();
            Assert.IsType<List<ReservationDto>>(result);
            Assert.Equal("/api/reservations", mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Get, mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task GetOne_IsConfiguredCorrectly()
        {
            mockHttpMessageHandler.ResponseObject = reservationDto;
            var result = await reservationApiClient.GetAsync(reservationDto.Id);
            Assert.IsType<ReservationDto>(result);
            Assert.Equal($"/api/reservations/{reservationDto.Id}", mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Get, mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task Post_IsConfiguredCorrectly()
        {
            mockHttpMessageHandler.ResponseObject = reservationDto;
            var result = await reservationApiClient.PostAsync(reservationDto);
            Assert.IsType<ReservationDto>(result);
            Assert.Equal("/api/reservations", mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Post, mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task Put_IsConfiguredCorrectly()
        {
            mockHttpMessageHandler.ResponseObject = null;
            await reservationApiClient.PutAsync(reservationDto);
            Assert.Equal($"/api/reservations/{reservationDto.Id}", mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Put, mockHttpMessageHandler.RequestMethod);
        }   

        [Fact]
        public async Task Delete_UsesCorrectHttpInfo()
        {
            mockHttpMessageHandler.ResponseObject = null;
            await reservationApiClient.DeleteAsync(reservationDto.Id);
            Assert.Equal($"/api/reservations/{reservationDto.Id}", mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Delete, mockHttpMessageHandler.RequestMethod);
        }
    }
}
