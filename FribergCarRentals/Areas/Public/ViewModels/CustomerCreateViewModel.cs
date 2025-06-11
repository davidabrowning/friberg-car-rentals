using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Areas.Public.ViewModels
{
    public class CustomerCreateViewModel
    {
        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public string HomeCity { get; set; } = null!;

        [Required]
        public string HomeCountry { get; set; } = null!;
    }
}
