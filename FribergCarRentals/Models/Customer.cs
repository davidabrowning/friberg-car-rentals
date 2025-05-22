namespace FribergCarRentals.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = new();
        List<Reservation> Reservations { get; set; } = new();
    }
}
