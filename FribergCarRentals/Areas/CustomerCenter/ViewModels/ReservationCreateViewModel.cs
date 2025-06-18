using FribergCarRentals.Models;

namespace FribergCarRentals.Areas.CustomerCenter.ViewModels
{
    public class ReservationCreateViewModel
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int CarId { get; set; }
        public int CustomerId { get; set; }
    }
}
