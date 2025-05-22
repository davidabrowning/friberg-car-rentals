namespace FribergCarRentals.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = new();
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        List<Reservation> Reservations { get; set; } = new();
    }
}
