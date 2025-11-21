using FribergCarRentals.Core.Models;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.WebApi.Mappers
{
    public static class CustomerMapper
    {
        public static CustomerDto ToDto(Customer customer)
        {
            CustomerDto customerDto = new()
            {
                Id = customer.Id,
                UserId = customer.UserId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                HomeCity = customer.HomeCity,
                HomeCountry = customer.HomeCountry
            };
            return customerDto;
        }

        public static Customer ToModel(CustomerDto customerDto)
        {
            Customer customer = new()
            {
                Id= customerDto.Id,
                UserId = customerDto.UserId,
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                HomeCity = customerDto.HomeCity,
                HomeCountry = customerDto.HomeCountry
            };
            return customer;
        }
    }
}