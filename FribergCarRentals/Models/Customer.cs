namespace FribergCarRentals.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        List<Reservation> Reservations { get; set; }
    }
}
