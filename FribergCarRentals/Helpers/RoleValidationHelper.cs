using Microsoft.AspNetCore.Identity;

namespace FribergCarRentals.Helpers
{
    public static class RoleValidationHelper
    {
        public static bool EmailAlreadyClaimed(string email, UserManager<IdentityUser> userManager)
        {
            return userManager.Users.Where(u => Equals(u.Email, email)).Any();
        }
    }
}
