using FribergCarRentals.Core.Models;
using FribergCarRentals.Mvc.ApiClients;
using FribergCarRentals.Tests.Mock.Other;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.Tests.Tests.Mvc.ApiClients
{
    public class CarApiClientTests
    {
        private MockHttpMessageHandler _mockHttpMessageHandler;
        private CarApiClient _carApiClient;

        public CarApiClientTests()
        {
            _mockHttpMessageHandler = new();
            HttpClient httpClient = new(_mockHttpMessageHandler) { BaseAddress = new Uri("http://localhost") };
            _carApiClient = new(httpClient);
        }

        [Fact]
        public async Task GetAll_IsConfiguredCorrectly()
        {
            _mockHttpMessageHandler.ResponseObject = new List<CarDto>();
            var result = await _carApiClient.GetAsync();
            Assert.IsType<List<CarDto>>(result);
            Assert.Equal("/api/cars", _mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Get, _mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task GetOne_IsConfiguredCorrectly()
        {
            CarDto carDto = new() { Id = 42 };
            _mockHttpMessageHandler.ResponseObject = carDto;
            var result = await _carApiClient.GetAsync(carDto.Id);
            Assert.IsType<CarDto>(result);
            Assert.Equal($"/api/cars/{carDto.Id}", _mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Get, _mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task Post_IsConfiguredCorrectly()
        {
            CarDto carDto = new() { Id = 42 };
            _mockHttpMessageHandler.ResponseObject = carDto;
            var result = await _carApiClient.PostAsync(carDto);
            Assert.IsType<CarDto>(result);
            Assert.Equal("/api/cars", _mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Post, _mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task Put_IsConfiguredCorrectly()
        {
            CarDto carDto = new() { Id = 42 };
            _mockHttpMessageHandler.ResponseObject = new CarDto();
            var result = await _carApiClient.PutAsync(carDto);
            Assert.IsType<CarDto>(result);
            Assert.Equal($"/api/cars/{carDto.Id}", _mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Put, _mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task Delete_IsConfiguredCorrectly()
        {
            CarDto carDto = new() { Id= 42 };
            _mockHttpMessageHandler.ResponseObject = new CarDto();
            await _carApiClient.DeleteAsync(42);
            Assert.Equal($"/api/cars/{carDto.Id}", _mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Delete, _mockHttpMessageHandler.RequestMethod);
        }
    }
}
