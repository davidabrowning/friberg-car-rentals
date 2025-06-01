using Microsoft.AspNetCore.Identity;

namespace FribergCarRentals.Areas.Administration.ViewModels
{
    public class AdminViewModel
    {
        public int Id { get; set; }
        public IdentityUser? IdentityUser { get; set; } = null;
    }
}
