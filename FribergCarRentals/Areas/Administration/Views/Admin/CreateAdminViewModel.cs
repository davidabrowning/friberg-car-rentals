using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Mvc.Areas.Administration.Views.Admin
{
    public class CreateAdminViewModel
    {
        [Required]
        public required string IdentityUserId { get; set; }
    }
}
