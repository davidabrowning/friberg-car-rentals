using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Mvc.Areas.Public.Views.Session
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
