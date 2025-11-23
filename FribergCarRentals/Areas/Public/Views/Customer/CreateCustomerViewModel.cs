using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Mvc.Areas.Public.Views.Customer
{
    public class CreateCustomerViewModel
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
