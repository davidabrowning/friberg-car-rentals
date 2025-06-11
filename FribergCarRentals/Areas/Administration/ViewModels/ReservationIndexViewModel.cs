using FribergCarRentals.Models;

namespace FribergCarRentals.Areas.Administration.ViewModels
{
    public class ReservationIndexViewModel
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Car? Car { get; set; }
        public Customer? Customer { get; set; }
        public List<Car> AllCars { get; set; } = new();
        public List<Customer> AllCustomers { get; set; } = new();
    }
}
