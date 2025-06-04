using Microsoft.AspNetCore.Identity;

namespace FribergCarRentals.Areas.Administration.ViewModels
{
    public class AdminViewModel
    {
        public int Id { get; set; }
        public string IdentityUserId { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }
}
