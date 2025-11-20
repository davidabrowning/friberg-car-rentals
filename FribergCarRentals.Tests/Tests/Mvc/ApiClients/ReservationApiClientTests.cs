using FribergCarRentals.Core.Models;
using FribergCarRentals.Mvc.ApiClients;
using FribergCarRentals.Tests.Mock.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.Tests.Tests.Mvc.ApiClients
{
    public class ReservationApiClientTests
    {
        // Reused variables
        private MockHttpMessageHandler mockHttpMessageHandler;
        private ReservationApiClient reservationApiClient;
        private List<Reservation>? reservations;

        public ReservationApiClientTests()
        {
            mockHttpMessageHandler = new();
            HttpClient httpClient = new(mockHttpMessageHandler) { BaseAddress = new Uri("http://localhost") };
            reservationApiClient = new(httpClient);
        }

        [Fact]
        public async Task Get_ReturnsReservations()
        {
            reservations = new() { new Reservation(), new Reservation() };
            mockHttpMessageHandler.ExpectedPath = "/api/reservations";
            mockHttpMessageHandler.ResponseObject = reservations;
            mockHttpMessageHandler.HttpStatusCode = HttpStatusCode.OK;
            IEnumerable<Reservation> result = await reservationApiClient.GetAsync();
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetId_ReturnsCorrectReservation()
        {
            Reservation otherReservation = new Reservation() { Id = 123 };
            Reservation targetReservation = new Reservation() { Id = 456 };
            reservations = new() { otherReservation, targetReservation };
            mockHttpMessageHandler.ExpectedPath = $"/api/reservations/{targetReservation.Id}";
            mockHttpMessageHandler.ResponseObject = targetReservation;
            mockHttpMessageHandler.HttpStatusCode = HttpStatusCode.OK;
            Reservation? result = await reservationApiClient.GetAsync(targetReservation.Id);
            Assert.Equal(targetReservation, result);
        }
    }
}
