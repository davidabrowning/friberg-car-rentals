using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;
using FribergCarRentals.WebApi.Dtos;
using FribergCarRentals.WebApi.Mappers;
using Microsoft.AspNetCore.Mvc;

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
            Customer customer = CustomerMapper.ToModel(customerDto);
            await _userService.CreateCustomerAsync(customer);
            return Ok(customerDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CustomerDto>> Put(int id, CustomerDto customerDto)
        {
            Customer customer = CustomerMapper.ToModel(customerDto);
            await _userService.UpdateCustomerAsync(customer);
            return Ok(customerDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CustomerDto?>> Delete(int id)
        {
            Customer? customer = await _userService.DeleteCustomerAsync(id);
            if (customer == null)
            {
                return BadRequest("Unable to find customer with that id.");
            }
            CustomerDto customerDto = CustomerMapper.ToDto(customer);
            return Ok(customerDto);
        }
    }
}
