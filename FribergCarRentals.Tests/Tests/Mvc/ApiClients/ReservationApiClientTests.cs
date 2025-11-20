using FribergCarRentals.Core.Models;
using FribergCarRentals.Mvc.ApiClients;
using FribergCarRentals.Tests.Mock.Other;

namespace FribergCarRentals.Tests.Tests.Mvc.ApiClients
{
    public class ReservationApiClientTests
    {
        // Reused variables
        private MockHttpMessageHandler mockHttpMessageHandler;
        private ReservationApiClient reservationApiClient;
        private List<Reservation> reservations;
        private Reservation reservation;

        public ReservationApiClientTests()
        {
            mockHttpMessageHandler = new();
            HttpClient httpClient = new(mockHttpMessageHandler) { BaseAddress = new Uri("http://localhost") };
            reservationApiClient = new(httpClient);
            reservations = new() { new Reservation() { Id = 27 }, new Reservation() { Id = 28 } };
            reservation = new() { Id = 42 };
        }

        [Fact]
        public async Task GetAll_IsConfiguredCorrectly()
        {
            mockHttpMessageHandler.ResponseObject = reservations;
            var result = await reservationApiClient.GetAsync();
            Assert.IsType<List<Reservation>>(result);
            Assert.Equal("/api/reservations", mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Get, mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task GetOne_IsConfiguredCorrectly()
        {
            mockHttpMessageHandler.ResponseObject = reservation;
            var result = await reservationApiClient.GetAsync(reservation.Id);
            Assert.IsType<Reservation>(result);
            Assert.Equal($"/api/reservations/{reservation.Id}", mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Get, mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task Post_IsConfiguredCorrectly()
        {
            mockHttpMessageHandler.ResponseObject = new Reservation();
            var result = await reservationApiClient.PostAsync(new Reservation());
            Assert.IsType<Reservation>(result);
            Assert.Equal("/api/reservations", mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Post, mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task Put_IsConfiguredCorrectly()
        {
            reservation = new();
            mockHttpMessageHandler.ResponseObject = null;
            await reservationApiClient.PutAsync(reservation);
            Assert.Equal($"/api/reservations/{reservation.Id}", mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Put, mockHttpMessageHandler.RequestMethod);
        }   

        [Fact]
        public async Task Delete_UsesCorrectHttpInfo()
        {
            mockHttpMessageHandler.ResponseObject = null;
            await reservationApiClient.DeleteAsync(reservation.Id);
            Assert.Equal($"/api/reservations/{reservation.Id}", mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Delete, mockHttpMessageHandler.RequestMethod);
        }
    }
}
