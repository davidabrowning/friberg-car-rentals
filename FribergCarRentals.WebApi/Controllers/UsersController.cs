using FribergCarRentals.Core.Interfaces.Facades;
using FribergCarRentals.Services.ApplicationModels;
using FribergCarRentals.WebApi.Dtos;
using FribergCarRentals.WebApi.Mappers;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<IEnumerable<UserDto>>> Get()
        {
            IEnumerable<string> userIds = await _applicationFacade.GetAllUserIdsAsync();
            List<UserInfoModel> userInfoModels = new();
            foreach (string userId in userIds)
            {
                UserInfoModel? userInfoModel = await BuildUserInfoModelAsync(userId);
                if (userInfoModel != null)
                    userInfoModels.Add(userInfoModel);
            }
            IEnumerable<UserDto> userDtos = UserMapper.ToDtosAsync(userInfoModels);
            return Ok(userDtos);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDto>> Get(string userId)
        {
            UserInfoModel? userInfoModel = await BuildUserInfoModelAsync(userId);
            if (userInfoModel == null)
                return NotFound();
            UserDto userDto = UserMapper.ToDtoAsync(userInfoModel);
            return Ok(userDto);
        }

        [HttpGet("username-exists/{username}")]
        public async Task<ActionResult<bool>> UsernameExists(string username)
        {
            string? userId = await _applicationFacade.GetUserIdAsync(username);
            if (userId == null)
                return Ok(false);
            return Ok(true);
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<UserDto>> GetFromUsername(string username)
        {
            string? userId = await _applicationFacade.GetUserIdAsync(username);
            if (userId == null)
                return NotFound();
            UserInfoModel? userInfoModel = await BuildUserInfoModelAsync(userId);
            if (userInfoModel == null) 
                return NotFound();
            UserDto userDto = UserMapper.ToDtoAsync(userInfoModel);
            return Ok(userDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult<JwtTokenDto>> Login([FromBody] LoginDto loginDto)
        {
            string? userId = await _applicationFacade.AuthenticateAsync(loginDto.Username, loginDto.Password);
            if (userId == null)
                return NotFound();
            IEnumerable<string> roles = await _applicationFacade.GetRolesAsync(userId);
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
            UserInfoModel? userInfoModel = await BuildUserInfoModelAsync(userId);
            if (userInfoModel == null) 
                return NotFound();
            UserDto userDto = UserMapper.ToDtoAsync(userInfoModel);
            return Ok(userDto);
        }

        [Authorize]
        [HttpPut("update-username/{userId}")]
        public async Task<ActionResult> UpdateUsername(string userId, string newUsername)
        {
            string? updatedUserId = await _applicationFacade.UpdateUsernameAsync(userId, newUsername);
            if (updatedUserId == null)
                return NotFound();
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(string username)
        {
            string? userId = await _applicationFacade.GetUserIdAsync(username);
            if (userId == null)
                return NotFound();
            await _applicationFacade.DeleteApplicationUserAsync(username);
            return NoContent();
        }

        private async Task<UserInfoModel?> BuildUserInfoModelAsync(string userId)
        {
            UserInfoModel userInfoModel = new()
            {
                UserId = userId,
                Username = await _applicationFacade.GetUsernameAsync(userId),
                AuthRoles = await _applicationFacade.GetRolesAsync(userId),
                Admin = await _applicationFacade.GetAdminAsync(userId),
                Customer = await _applicationFacade.GetCustomerAsync(userId),
            };
            return userInfoModel;
        }
    }
}
