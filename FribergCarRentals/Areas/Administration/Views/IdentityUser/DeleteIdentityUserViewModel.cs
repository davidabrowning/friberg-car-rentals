using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Areas.Administration.Views.IdentityUser
{
    public class DeleteIdentityUserViewModel
    {
        [Required]
        public string IdentityUserId { get; set; } = null!;
        [Required]
        public string IdentityUserUsername { get; set; } = null!;
    }
}
