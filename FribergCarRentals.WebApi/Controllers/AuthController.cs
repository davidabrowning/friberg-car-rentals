using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;
using FribergCarRentals.WebApi.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FribergCarRentals.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        public AuthController(IAuthService authService, IUserService userService, IJwtService jwtService)
        {
            _authService = authService;
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpGet("current-user-id")]
        public async Task<SignedInUserDto> GetCurrentSignedInUserIdAsync()
        {
            string? currentSignedInUser = await _userService.GetCurrentUserIdAsync();
            SignedInUserDto signedInUserDto = new() { UserId = currentSignedInUser };
            return signedInUserDto;
        }

        [HttpGet("current-user")]
        public async Task<SignedInUserDto?> GetCurrentUser()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string? username = User.Identity?.Name;

            if (userId == null || username == null)
            {
                return null;
            }
            return new SignedInUserDto
            {
                UserId = userId,
                Username = username
            };
        }

        [HttpGet("roles")]
        public async Task<List<string>> GetAuthRoles()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return new();
            }
            return await _authService.GetRolesAsync(userId);
        }

        [HttpGet("is-admin/{userId}")]
        public async Task<bool> IsAdmin(string userId)
        {
            Admin? admin = await _userService.GetAdminByUserIdAsync(userId);
            return admin != null;
        }

        [HttpGet("is-customer/{userId}")]
        public async Task<bool> IsCustomer(string userId)
        {
            Customer? customer = await _userService.GetCustomerByUserIdAsync(userId);
            return customer != null;
        }

        [HttpGet("is-user/{userId}")]
        public async Task<bool> IsUser(string userId)
        {
            string? username = await _userService.GetUsernameByUserIdAsync(userId);
            return username != null;
        }

        [HttpGet("is-in-role/{userId}/{roleName}")]
        public async Task<bool> IsInRoleName(string userId, string roleName)
        {
            return await _userService.IsInRoleAsync(userId, roleName);
        }

        [HttpGet("admin-id/{userId}")]
        public async Task<int> GetAdminIdByUserId(string userId)
        {
            Admin admin = await _userService.GetAdminByUserIdAsync(userId);
            return admin.Id;
        }

        [HttpGet("customer-id/{userId}")]
        public async Task<int> GetCustomerIdByUserId(string userId)
        {
            Customer customer = await _userService.GetCustomerByUserIdAsync(userId);
            return customer.Id;
        }

        [HttpGet("username/{userId}")]
        public async Task<string> GetUsernameByUserId(string userId)
        {
            return await _userService.GetUsernameByUserIdAsync(userId);
        }

        [HttpGet("get-all-user-ids")]
        public async Task<List<string>> GetAllUserIds()
        {
            return await _userService.GetAllUserIdsAsync();
        }

        [HttpPost("create-user")]
        public async Task CreateUser(string username)
        {
            await _userService.CreateUserAsync(username);
        }

        [HttpPost("login")]
        public async Task<JwtTokenDto?> Login([FromBody] LoginDto loginDto)
        {
            string? userId = await _authService.AuthenticateUserAsync(loginDto.Username, loginDto.Password);
            if (userId == null)
            {
                return null;
            }

            List<string> roles = await _authService.GetRolesAsync(userId);
            var token = _jwtService.GenerateJwtToken(userId, loginDto.Username, roles);

            return new JwtTokenDto { Token = token };
        }

        [HttpPost("register")]
        public async Task<string?> Register([FromBody] RegisterDto registerDto)
        {
            string? userId = await _authService.CreateUserWithPasswordAsync(registerDto.Username, registerDto.Password);
            return userId;
        }

        [HttpPut("update-username/{userId}")]
        public async Task UpdateUsername(string userId, string newUsername)
        {
            await _userService.UpdateUsernameAsync(userId, newUsername);
        }

        [HttpDelete("delete-user/{userId}")]
        public async Task DeleteUser(string username)
        {
            await _userService.DeleteUserAsync(username);
        }
    }
}
