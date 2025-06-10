using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Areas.Administration.ViewModels
{
    public class AdminCreateViewModel
    {
        [Required]
        public required string IdentityUserId { get; set; }
    }
}
