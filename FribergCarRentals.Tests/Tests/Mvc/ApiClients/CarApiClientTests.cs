using FribergCarRentals.Core.Models;
using FribergCarRentals.Mvc.ApiClients;
using FribergCarRentals.Tests.Mock.Other;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.Tests.Tests.Mvc.ApiClients
{
    public class CarApiClientTests
    {
        private MockHttpMessageHandler mockHttpMessageHandler;
        private CarApiClient carApiClient;

        public CarApiClientTests()
        {
            mockHttpMessageHandler = new();
            HttpClient httpClient = new(mockHttpMessageHandler) { BaseAddress = new Uri("http://localhost") };
            carApiClient = new(httpClient);
        }

        [Fact]
        public async Task GetAll_IsConfiguredCorrectly()
        {
            mockHttpMessageHandler.ResponseObject = new List<CarDto>();
            var result = await carApiClient.GetAsync();
            Assert.IsType<List<CarDto>>(result);
            Assert.Equal("/api/cars", mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Get, mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task GetOne_IsConfiguredCorrectly()
        {
            CarDto carDto = new() { Id = 42 };
            mockHttpMessageHandler.ResponseObject = carDto;
            var result = await carApiClient.GetAsync(carDto.Id);
            Assert.IsType<CarDto>(result);
            Assert.Equal($"/api/cars/{carDto.Id}", mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Get, mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task Post_IsConfiguredCorrectly()
        {
            CarDto carDto = new() { Id = 42 };
            mockHttpMessageHandler.ResponseObject = carDto;
            var result = await carApiClient.PostAsync(carDto);
            Assert.IsType<CarDto>(result);
            Assert.Equal("/api/cars", mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Post, mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task Put_IsConfiguredCorrectly()
        {
            CarDto carDto = new() { Id = 42 };
            mockHttpMessageHandler.ResponseObject = null;
            await carApiClient.PutAsync(carDto);
            Assert.Equal($"/api/cars/{carDto.Id}", mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Put, mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task Delete_UsesCorrectHttpInfo()
        {
            CarDto carDto = new() { Id= 42 };
            mockHttpMessageHandler.ResponseObject = null;
            await carApiClient.DeleteAsync(carDto.Id);
            Assert.Equal($"/api/cars/{carDto.Id}", mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Delete, mockHttpMessageHandler.RequestMethod);
        }
    }
}
