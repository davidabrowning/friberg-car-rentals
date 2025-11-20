using FribergCarRentals.Core.Models;
using FribergCarRentals.Mvc.ApiClients;
using FribergCarRentals.Tests.Mock.Other;

namespace FribergCarRentals.Tests.Tests.Mvc.ApiClients
{
    public class CarApiClientTests
    {
        // Reused variables
        private MockHttpMessageHandler mockHttpMessageHandler;
        private CarApiClient carApiClient;
        private List<Car> cars;
        private Car car;

        public CarApiClientTests()
        {
            mockHttpMessageHandler = new();
            HttpClient httpClient = new(mockHttpMessageHandler) { BaseAddress = new Uri("http://localhost") };
            carApiClient = new(httpClient);
            cars = new() { new Car() { Id = 27 }, new Car() { Id = 28 } };
            car = new() { Id = 42 };
        }

        [Fact]
        public async Task GetAll_IsConfiguredCorrectly()
        {
            mockHttpMessageHandler.ResponseObject = cars;
            var result = await carApiClient.GetAsync();
            Assert.IsType<List<Car>>(result);
            Assert.Equal("/api/cars", mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Get, mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task GetOne_IsConfiguredCorrectly()
        {
            mockHttpMessageHandler.ResponseObject = car;
            var result = await carApiClient.GetAsync(car.Id);
            Assert.IsType<Car>(result);
            Assert.Equal($"/api/cars/{car.Id}", mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Get, mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task Post_IsConfiguredCorrectly()
        {
            mockHttpMessageHandler.ResponseObject = car;
            var result = await carApiClient.PostAsync(car);
            Assert.IsType<Car>(result);
            Assert.Equal("/api/cars", mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Post, mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task Put_IsConfiguredCorrectly()
        {
            mockHttpMessageHandler.ResponseObject = null;
            await carApiClient.PutAsync(car);
            Assert.Equal($"/api/cars/{car.Id}", mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Put, mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task Delete_UsesCorrectHttpInfo()
        {
            mockHttpMessageHandler.ResponseObject = null;
            await carApiClient.DeleteAsync(car.Id);
            Assert.Equal($"/api/cars/{car.Id}", mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Delete, mockHttpMessageHandler.RequestMethod);
        }
    }
}
