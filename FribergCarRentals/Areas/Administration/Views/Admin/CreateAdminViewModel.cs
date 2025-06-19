using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Areas.Administration.Views.Admin
{
    public class CreateAdminViewModel
    {
        [Required]
        public required string IdentityUserId { get; set; }
    }
}
