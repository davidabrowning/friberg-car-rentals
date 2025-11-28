using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.Facades;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;
using FribergCarRentals.WebApi.Dtos;
using FribergCarRentals.WebApi.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly IApplicationFacade _applicationFacade;
        public AdminsController(IApplicationFacade applicationFacade)
        {
            _applicationFacade = applicationFacade; 
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AdminDto>> Get(int id)
        {
            Admin? admin = await _applicationFacade.GetAdminAsync(id);
            if (admin == null)
                return NotFound();
            AdminDto adminDto = AdminMapper.ToDto(admin);
            return Ok(adminDto);
        }

        [HttpPost]
        public async Task<ActionResult<AdminDto>> Post(AdminDto adminDto)
        {
            Admin admin = AdminMapper.ToNewModelWIthoutId(adminDto);
            await _applicationFacade.CreateAdminAsync(admin);
            AdminDto resultAdminDto = AdminMapper.ToDto(admin);
            return Ok(resultAdminDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, AdminDto adminDto)
        {
            if (id != adminDto.Id)
                return BadRequest(UserMessage.ErrorIdsDoNotMatch);
            Admin? admin = await _applicationFacade.GetAdminAsync(id);
            if (admin == null)
                return NotFound();
            AdminMapper.UpdateModel(admin, adminDto);
            await _applicationFacade.UpdateAdminAsync(admin);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            Admin? admin = await _applicationFacade.DeleteAdminAsync(id);
            if (admin == null)
                return NotFound();
            return NoContent();
        }
    }
}
