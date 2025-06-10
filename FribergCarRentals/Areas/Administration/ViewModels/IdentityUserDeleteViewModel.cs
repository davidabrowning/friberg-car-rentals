using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Areas.Administration.ViewModels
{
    public class IdentityUserDeleteViewModel
    {
        [Required]
        public string IdentityUserId { get; set; } = null!;
        [Required]
        public string IdentityUserUsername { get; set; } = null!;
    }
}
