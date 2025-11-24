using FribergCarRentals.Core.Helpers;
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
        public async Task<ActionResult<CustomerDto>> Get(int id)
        {
            Customer? customer = await _userService.GetCustomerByCustomerIdAsync(id);
            if (customer == null)
                return NotFound();
            CustomerDto customerDto = CustomerMapper.ToDto(customer);
            return Ok(customerDto);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Post(CustomerDto customerDto)
        {
            Customer customer = CustomerMapper.ToNewModelWIthoutId(customerDto);
            await _userService.CreateCustomerAsync(customer);
            CustomerDto resultCustomerDto = CustomerMapper.ToDto(customer);
            return Ok(resultCustomerDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, CustomerDto customerDto)
        {
            if (id != customerDto.Id)
                return BadRequest(UserMessage.ErrorIdsDoNotMatch);
            Customer? customer = await _userService.GetCustomerByCustomerIdAsync(id);
            if (customer == null)
                return NotFound();
            CustomerMapper.UpdateModel(customer, customerDto);
            await _userService.UpdateCustomerAsync(customer);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            Customer? customer = await _userService.DeleteCustomerAsync(id);
            if (customer == null)
                return NotFound();
            return NoContent();
        }
    }
}
