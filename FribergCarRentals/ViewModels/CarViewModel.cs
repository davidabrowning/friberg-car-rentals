using FribergCarRentals.Models;

namespace FribergCarRentals.ViewModels
{
    public class CarViewModel
    {
        public int Id { get; set; }
        public string Make { get; set; } = "";
        public string Model { get; set; } = "";
        public int Year { get; set; } = 0;
        public string Description { get; set; } = "";
        public List<Reservation> Reservations { get; set; } = new();
    }
}
