using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity;

namespace FribergCarRentals.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public IdentityUser? IdentityUser { get; set; } = null;
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string HomeCity { get; set; } = "";
        public string HomeCountry { get; set; } = "";
        public List<Reservation> Reservations { get; set; } = new();
    }
}
