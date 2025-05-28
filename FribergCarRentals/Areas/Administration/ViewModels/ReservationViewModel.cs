using FribergCarRentals.Models;

namespace FribergCarRentals.Areas.Administration.ViewModels
{
    public class ReservationViewModel
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Car Car { get; set; } = new();
        public Customer Customer { get; set; } = new();
        public List<Car> AllCars { get; set; } = new();
        public List<Customer> AllCustomers { get; set; } = new();
    }
}
