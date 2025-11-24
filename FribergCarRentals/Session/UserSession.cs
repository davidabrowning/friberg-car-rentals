using FribergCarRentals.Core.Constants;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;
using System.Threading.Tasks;

namespace FribergCarRentals.Mvc.Session
{
    public class UserSession
    {
        private readonly IUserApiClient _userApiClient;
        public UserDto UserDto { get; set; } = new();

        public UserSession(IUserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }
        
        public bool IsSignedIn()
        {
            return UserDto.UserId != null;
        }
        public bool IsAdmin()
        {
            return UserDto.AuthRoles.Where(r => r == AuthRoleName.Admin).Any();
        }
        public bool IsCustomer()
        {
            return UserDto.AuthRoles.Where(r => r == AuthRoleName.Customer).Any();
        }
        public void SignOut()
        {
            UserDto = new();
        }
        public async Task UpdateDto()
        {
            UserDto newUserDto = await _userApiClient.GetAsync(UserDto.UserId);
            UserDto = newUserDto;
        }
    }
}
