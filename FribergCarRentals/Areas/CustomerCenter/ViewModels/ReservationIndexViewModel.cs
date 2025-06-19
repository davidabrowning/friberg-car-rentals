using FribergCarRentals.Models;

namespace FribergCarRentals.Areas.CustomerCenter.ViewModels
{
    public class ReservationIndexViewModel
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Car Car { get; set; }
    }
}
