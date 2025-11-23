using FribergCarRentals.Mvc.ApiClients;
using FribergCarRentals.Tests.Mock.Other;
using FribergCarRentals.WebApi.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.Tests.Tests.Mvc.ApiClients
{
    public class AdminApiClientTests
    {
        private MockHttpMessageHandler _mockHttpMessageHandler;
        private AdminApiClient _adminApiClient;

        public AdminApiClientTests()
        {
            _mockHttpMessageHandler = new();
            HttpClient httpClient = new(_mockHttpMessageHandler) { BaseAddress = new Uri("http://localhost") };
            _adminApiClient = new(httpClient);
        }

        [Fact]
        public async Task GetAll_IsConfiguredCorrectly()
        {
            _mockHttpMessageHandler.ResponseObject = new List<AdminDto>();
            var result = await _adminApiClient.GetAsync();
            Assert.IsType<List<AdminDto>>(result);
            Assert.Equal("/api/admins", _mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Get, _mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task GetOne_IsConfiguredCorrectly()
        {
            AdminDto adminDto = new() { Id = 42 };
            _mockHttpMessageHandler.ResponseObject = adminDto;
            var result = await _adminApiClient.GetAsync(adminDto.Id);
            Assert.IsType<AdminDto>(result);
            Assert.Equal($"/api/admins/{adminDto.Id}", _mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Get, _mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task Post_IsConfiguredCorrectly()
        {
            AdminDto adminDto = new() { Id = 42 };
            _mockHttpMessageHandler.ResponseObject = adminDto;
            var result = await _adminApiClient.PostAsync(adminDto);
            Assert.IsType<AdminDto>(result);
            Assert.Equal("/api/admins", _mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Post, _mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task Put_IsConfiguredCorrectly()
        {
            AdminDto adminDto = new() { Id = 42 };
            _mockHttpMessageHandler.ResponseObject = new AdminDto();
            var result = await _adminApiClient.PutAsync(adminDto);
            Assert.IsType<AdminDto>(result);
            Assert.Equal($"/api/admins/{adminDto.Id}", _mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Put, _mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task Delete_IsConfiguredCorrectly()
        {
            AdminDto adminDto = new() { Id = 42 };
            _mockHttpMessageHandler.ResponseObject = new AdminDto();
            var result = await _adminApiClient.DeleteAsync(adminDto.Id);
            Assert.IsType<AdminDto>(result);
            Assert.Equal($"/api/admins/{adminDto.Id}", _mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Delete, _mockHttpMessageHandler.RequestMethod);
        }
    }
}
