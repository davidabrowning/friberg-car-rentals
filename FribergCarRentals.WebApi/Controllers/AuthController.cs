using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;
using FribergCarRentals.WebApi.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("current-user-id")]
        public async Task<SignedInUserDto> GetCurrentSignedInUserIdAsync()
        {
            string? currentSignedInUser = await _userService.GetCurrentUserIdAsync();
            SignedInUserDto signedInUserDto = new() { UserId = currentSignedInUser };
            return signedInUserDto;
        }

        [HttpGet("current-user")]
        public async Task<SignedInUserDto> GetCurrentUser()
        {
            throw new NotImplementedException();
        }

        [HttpGet("roles")]
        public async Task GetAuthRoles()
        {
            throw new NotImplementedException();
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
        public async Task Login()
        {
            throw new NotImplementedException();
        }

        [HttpPost("")]
        public async Task Register()
        {
            throw new NotImplementedException();
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
