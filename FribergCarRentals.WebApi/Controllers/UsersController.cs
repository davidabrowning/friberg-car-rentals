using FribergCarRentals.Core.Interfaces.Facades;
using FribergCarRentals.WebApi.Dtos;
using FribergCarRentals.WebApi.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IApplicationFacade _applicationFacade;
        public UsersController(IApplicationFacade applicationFacade)
        {
            _applicationFacade = applicationFacade;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> Get()
        {
            List<string> userIds = await _applicationFacade.GetAllUserIdsAsync();
            List<UserDto> userDtos = await UserMapper.ToDtosAsync(userIds, _applicationFacade);
            return Ok(userDtos);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDto>> Get(string userId)
        {
            string? username = await _applicationFacade.GetUsernameAsync(userId);
            if (username == null)
                return NotFound();
            UserDto userDto = await UserMapper.ToDtoAsync(userId, _applicationFacade);
            return Ok(userDto);
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<UserDto>> GetFromUsername(string username)
        {
            string? userId = await _applicationFacade.GetUserIdAsync(username);
            if (userId == null)
                return NotFound();
            UserDto userDto = await UserMapper.ToDtoAsync(userId, _applicationFacade);
            return Ok(userDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult<JwtTokenDto>> Login([FromBody] LoginDto loginDto)
        {
            string? userId = await _applicationFacade.AuthenticateAsync(loginDto.Username, loginDto.Password);
            if (userId == null)
                return NotFound();
            List<string> roles = await _applicationFacade.GetRolesAsync(userId);
            string token = _applicationFacade.GenerateJwtToken(userId, loginDto.Username, roles);
            JwtTokenDto jwtTokenDto = new() { Token = token };
            return Ok(jwtTokenDto);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
        {
            string? userId = await _applicationFacade.CreateApplicationUserAsync(registerDto.Username, registerDto.Password);
            if (userId == null)
                return NotFound();
            UserDto userDto = await UserMapper.ToDtoAsync(userId, _applicationFacade);
            return Ok(userDto);
        }

        [HttpPut("update-username/{userId}")]
        public async Task<ActionResult> UpdateUsername(string userId, string newUsername)
        {
            string? updatedUserId = await _applicationFacade.UpdateUsernameAsync(userId, newUsername);
            if (updatedUserId == null)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(string username)
        {
            string? userId = await _applicationFacade.GetUserIdAsync(username);
            if (userId == null)
                return NotFound();
            await _applicationFacade.DeleteApplicationUserAsync(username);
            return NoContent();
        }
    }
}
