using Microsoft.AspNetCore.Identity;

namespace FribergCarRentals.ViewModels
{
    public class AdminViewModel
    {
        public int Id { get; set; }
        public IdentityUser? IdentityUser { get; set; } = null;
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }
}
