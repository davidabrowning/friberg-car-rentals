using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;
using FribergCarRentals.WebApi.Dtos;
using FribergCarRentals.WebApi.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FribergCarRentals.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        public UsersController(IAuthService authService, IUserService userService, IJwtService jwtService)
        {
            _authService = authService;
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> Get()
        {
            List<string> userIds = await _userService.GetAllUserIdsAsync();
            List<UserDto> userDtos = await UserMapper.ToDtosAsync(userIds, _userService, _authService);
            return Ok(userDtos);
        }

        [HttpGet("current-user")]
        public ActionResult<UserDto> GetCurrentUser()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string? username = User.Identity?.Name;

            if (userId == null || username == null)
                return NotFound();
            UserDto signedInUserDto = new(){ UserId = userId, Username = username };
            return Ok(signedInUserDto);
        }

        [HttpPost("create-user")]
        public async Task<ActionResult<string>> CreateUser(string username)
        {
            await _userService.CreateUserAsync(username);
            string? userId = await _userService.GetUserIdByUsernameAsync(username);
            if (userId == null)
                return NotFound();
            return Ok(userId);
        }

        [HttpPost("login")]
        public async Task<ActionResult<JwtTokenDto>> Login([FromBody] LoginDto loginDto)
        {
            string? userId = await _authService.AuthenticateUserAsync(loginDto.Username, loginDto.Password);
            if (userId == null)
                return NotFound();
            List<string> roles = await _authService.GetRolesAsync(userId);
            string token = _jwtService.GenerateJwtToken(userId, loginDto.Username, roles);
            JwtTokenDto jwtTokenDto = new() { Token = token };
            return Ok(jwtTokenDto);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
        {
            string? userId = await _authService.CreateUserWithPasswordAsync(registerDto.Username, registerDto.Password);
            if (userId == null)
                return NotFound();
            UserDto userDto = await UserMapper.ToDtoAsync(userId, _userService, _authService);
            return Ok(userDto);
        }

        [HttpPut("update-username/{userId}")]
        public async Task<ActionResult> UpdateUsername(string userId, string newUsername)
        {
            string? updatedUserId = await _userService.UpdateUsernameAsync(userId, newUsername);
            if (updatedUserId == null) 
                return NotFound();
            return NoContent();
        }

        [HttpDelete("delete-user/{userId}")]
        public async Task<ActionResult> DeleteUser(string username)
        {
            string? deletedUserId = await _userService.DeleteUserAsync(username);
            if (deletedUserId == null)
                return NotFound();
            return NoContent();
        }
    }
}
