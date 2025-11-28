using FribergCarRentals.Core.Models;

namespace FribergCarRentals.Services.ApplicationModels
{
    public class UserInfoModel
    {
        public required string UserId;
        public string? Username;
        public required IEnumerable<string> AuthRoles;
        public Admin? Admin;
        public Customer? Customer;
    }
}
