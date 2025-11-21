using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;
using FribergCarRentals.WebApi.Dtos;
using FribergCarRentals.WebApi.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices.Marshalling;

namespace FribergCarRentals.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IUserService _userService;
        public CustomersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CustomerDto?>> Get(int id)
        {
            Customer? customer = await _userService.GetCustomerByCustomerIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            CustomerDto customerDto = CustomerMapper.ToDto(customer);
            return Ok(customerDto);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Post(CustomerDto customerDto)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CustomerDto>> Put(int id, CustomerDto customerDto)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CustomerDto?>> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
