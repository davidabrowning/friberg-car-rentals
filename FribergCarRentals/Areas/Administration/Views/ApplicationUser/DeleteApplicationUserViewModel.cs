using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Mvc.Areas.Administration.Views.ApplicationUser
{
    public class DeleteApplicationUserViewModel
    {
        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        public string Username { get; set; } = null!;
    }
}
