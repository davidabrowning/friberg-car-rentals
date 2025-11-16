namespace FribergCarRentals.Core.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Car? Car { get; set; }
        public Customer? Customer { get; set; }

        public override string? ToString()
        {
            return $"Confirmation ID #{Id} | {Customer.ToString()} | {Car.ToString()} | {StartDate} to {EndDate}";
        }
    }
}
