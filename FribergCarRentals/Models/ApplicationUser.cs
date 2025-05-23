using Microsoft.AspNetCore.Identity;

namespace FribergCarRentals.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Admin? Admin { get; set; } = null;
        public Customer? Customer { get; set; } = null;
    }
}
