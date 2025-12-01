using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.Facades;
using FribergCarRentals.Core.Models;
using FribergCarRentals.WebApi.Dtos;
using FribergCarRentals.WebApi.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IApplicationFacade _applicationFacade;
        public CustomersController(IApplicationFacade applicationFacade)
        {
            _applicationFacade = applicationFacade;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CustomerDto>> Get(int id)
        {
            Customer? customer = await _applicationFacade.GetCustomerAsync(id);
            if (customer == null)
                return NotFound();
            CustomerDto customerDto = CustomerMapper.ToDto(customer);
            return Ok(customerDto);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Post(CustomerDto customerDto)
        {
            Customer customer = CustomerMapper.ToNewModelWithoutId(customerDto);
            await _applicationFacade.CreateCustomerAsync(customer);
            CustomerDto resultCustomerDto = CustomerMapper.ToDto(customer);
            return Ok(resultCustomerDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, CustomerDto customerDto)
        {
            if (id != customerDto.Id)
                return BadRequest(UserMessage.ErrorIdsDoNotMatch);
            Customer? customer = await _applicationFacade.GetCustomerAsync(id);
            if (customer == null)
                return NotFound();
            CustomerMapper.UpdateModel(customer, customerDto);
            await _applicationFacade.UpdateCustomerAsync(customer);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            Customer? customer = await _applicationFacade.DeleteCustomerAsync(id);
            if (customer == null)
                return NotFound();
            return NoContent();
        }
    }
}
