using Microsoft.AspNetCore.Identity;

namespace FribergCarRentals.Areas.Administration.ViewModels
{
    public class AdminEditViewModel
    {
        public required int AdminId { get; set; }
        public required string IdentityUserUsername { get; set; }
    }
}
