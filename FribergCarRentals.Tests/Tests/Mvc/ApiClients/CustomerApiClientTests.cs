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
    public class CustomerApiClientTests
    {
        private MockHttpMessageHandler _mockHttpMessageHandler;
        private CustomerApiClient _customerApiClient;

        public CustomerApiClientTests()
        {
            _mockHttpMessageHandler = new();
            HttpClient httpClient = new(_mockHttpMessageHandler) { BaseAddress = new Uri("http://localhost") };
            _customerApiClient = new(httpClient);
        }

        [Fact]
        public async Task GetAll_IsConfiguredCorrectly()
        {
            _mockHttpMessageHandler.ResponseObject = new List<CustomerDto>();
            var result = await _customerApiClient.GetAsync();
            Assert.IsType<List<CustomerDto>>(result);
            Assert.Equal("/api/customers", _mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Get, _mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task GetOne_IsConfiguredCorrectly()
        {
            CustomerDto customerDto = new() { Id = 42 };
            _mockHttpMessageHandler.ResponseObject = customerDto;
            var result = await _customerApiClient.GetAsync(customerDto.Id);
            Assert.IsType<CustomerDto>(result);
            Assert.Equal($"/api/customers/{customerDto.Id}", _mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Get, _mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task Post_IsConfiguredCorrectly()
        {
            CustomerDto customerDto = new() { Id = 42 };
            _mockHttpMessageHandler.ResponseObject = customerDto;
            var result = await _customerApiClient.PostAsync(customerDto);
            Assert.IsType<CustomerDto>(result);
            Assert.Equal("/api/customers", _mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Post, _mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task Put_IsConfiguredCorrectly()
        {
            CustomerDto customerDto = new() { Id = 42 };
            _mockHttpMessageHandler.ResponseObject = new CustomerDto();
            var result = await _customerApiClient.PutAsync(customerDto);
            Assert.IsType<CustomerDto>(result);
            Assert.Equal($"/api/customers/{customerDto.Id}", _mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Put, _mockHttpMessageHandler.RequestMethod);
        }

        [Fact]
        public async Task Delete_IsConfiguredCorrectly()
        {
            CustomerDto customerDto = new() { Id = 42 };
            _mockHttpMessageHandler.ResponseObject = new CustomerDto();
            Assert.Equal($"/api/customers/{customerDto.Id}", _mockHttpMessageHandler.RequestPath);
            Assert.Equal(HttpMethod.Delete, _mockHttpMessageHandler.RequestMethod);
        }
    }
}
