namespace FribergCarRentals.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Car Car { get; set; } = new();
        public Customer Customer { get; set; } = new();
    }
}
