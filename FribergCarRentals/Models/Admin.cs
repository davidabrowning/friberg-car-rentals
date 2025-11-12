using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
    }
}
