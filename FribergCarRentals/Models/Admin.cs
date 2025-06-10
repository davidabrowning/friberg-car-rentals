using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public required IdentityUser IdentityUser { get; set; }
    }
}
