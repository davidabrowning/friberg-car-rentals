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
            string? currentSignedInUser = await _userService.GetCurrentUserId();
            SignedInUserDto signedInUserDto = new() { UserId = currentSignedInUser };
            return signedInUserDto;
        }

        [HttpGet("is-customer/{userId}")]
        public async Task<bool> IsCustomer(string userId)
        {
            Customer? customer = await _userService.GetCustomerByUserIdAsync(userId);
            return customer != null;
        }
    }
}
