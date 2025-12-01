using FribergCarRentals.Core.Constants;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.Mvc.Session
{
    public class UserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserApiClient _userApiClient;
        public UserSession(IHttpContextAccessor httpContextAccessor, IUserApiClient userApiClient)
        {
            _httpContextAccessor = httpContextAccessor;
            _userApiClient = userApiClient;
        }

        public async Task<UserDto> GetUserDto()
        {
            string? token = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];
            if (token == null)
                return new UserDto();
            string? userId = JwtReader.GetUserId(token);
            if (userId == null)
                return new UserDto();
            try
            {
                UserDto userDto = await _userApiClient.GetAsync(userId);
                return userDto;
            }
            catch
            {
                return new UserDto();
            }
        }
        
        public bool IsSignedIn()
        {
            string? token = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];
            return token != null;
        }
        public bool IsAdmin()
        {
            string? token = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];
            if (token == null)
                return false;
            IEnumerable<string> roles = JwtReader.GetRoles(token);
            if (roles.Where(r => r == AuthRoleName.Admin).Any())
                return true;
            return false;
        }
        public bool IsCustomer()
        {
            string? token = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];
            if (token == null)
                return false;
            IEnumerable<string> roles = JwtReader.GetRoles(token);
            if (roles.Where(r => r == AuthRoleName.Customer).Any())
                return true;
            return false;
        }
    }
}
