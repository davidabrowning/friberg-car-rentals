using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Mvc.Areas.Public.Views.Session
{
    public class SigninViewModel
    {
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
