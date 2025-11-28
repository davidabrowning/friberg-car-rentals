using FribergCarRentals.Core.Models;

namespace FribergCarRentals.Services.ApplicationModels
{
    public class UserInfoModel
    {
        public required string UserId;
        public required string Username;
        public List<string> AuthRoles = new();
        public Admin? Admin;
        public Customer? Customer;
    }
}
