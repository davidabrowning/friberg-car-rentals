using AspNetCoreGeneratedDocument;
using FribergCarRentals.Core.Constants;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.Mvc.Session
{
    public class UserSession
    {
        public UserDto UserDto { get; set; } = new();
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
    }
}
