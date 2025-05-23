using FribergCarRentals.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FribergCarRentals.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null;
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string HomeCity { get; set; } = "";
        public string HomeCountry { get; set; } = "";
        public List<Reservation> Reservations { get; set; } = new();
    }
}
