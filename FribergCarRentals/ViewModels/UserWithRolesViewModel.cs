using FribergCarRentals.Models;

namespace FribergCarRentals.ViewModels
{
    public class UserWithRolesViewModel
    {
        public required ApplicationUser ApplicationUser { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsUser { get; set; }
    }
}
