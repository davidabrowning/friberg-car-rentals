
namespace FribergCarRentals.Core.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Car? Car { get; set; }
        public Customer? Customer { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Reservation reservation &&
                   Id == reservation.Id &&
                   StartDate.Equals(reservation.StartDate) &&
                   EndDate.Equals(reservation.EndDate) &&
                   EqualityComparer<Car?>.Default.Equals(Car, reservation.Car) &&
                   EqualityComparer<Customer?>.Default.Equals(Customer, reservation.Customer);
        }

        public override string? ToString()
        {
            return $"Confirmation ID #{Id} | {Customer.ToString()} | {Car.ToString()} | {StartDate} to {EndDate}";
        }

    }
}
