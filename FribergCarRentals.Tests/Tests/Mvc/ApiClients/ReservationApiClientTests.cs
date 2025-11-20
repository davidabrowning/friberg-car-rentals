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
        private MockHttpMessageHandler? mockHttpMessageHandler;
        private HttpClient? httpClient;
        private List<Reservation>? reservations;
        private ReservationApiClient? reservationApiClient;

        [Fact]
        public async Task Get_ReturnsReservations()
        {
            reservations = new() { new Reservation(), new Reservation() };
            mockHttpMessageHandler = new("/api/reservations", reservations, HttpStatusCode.OK);
            httpClient = new HttpClient(mockHttpMessageHandler)
            {
                BaseAddress = new Uri("http://localhost")
            };
            reservationApiClient = new(httpClient);
            IEnumerable<Reservation> result = await reservationApiClient.GetAsync();
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetId_ReturnsCorrectReservation()
        {
            Reservation otherReservation = new Reservation() { Id = 123 };
            Reservation targetReservation = new Reservation() { Id = 456 };
            reservations = new() { otherReservation, targetReservation };
            mockHttpMessageHandler = new($"/api/reservations/{targetReservation.Id}", targetReservation, HttpStatusCode.OK);
            httpClient = new HttpClient(mockHttpMessageHandler)
            {
                BaseAddress = new Uri("http://localhost")
            };
            reservationApiClient = new(httpClient);
            Reservation? result = await reservationApiClient.GetAsync(targetReservation.Id);
            Assert.Equal(targetReservation, result);
        }
    }
}
