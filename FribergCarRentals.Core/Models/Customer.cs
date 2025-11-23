namespace FribergCarRentals.Core.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string HomeCity { get; set; } = "";
        public string HomeCountry { get; set; } = "";
        public List<Reservation> Reservations { get; set; } = new();

        public override bool Equals(object? obj)
        {
            return obj is Customer customer &&
                   Id == customer.Id;
        }
    }
}
