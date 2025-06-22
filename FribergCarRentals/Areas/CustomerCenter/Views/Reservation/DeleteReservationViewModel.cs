using FribergCarRentals.Models;

namespace FribergCarRentals.Areas.CustomerCenter.Views.Reservation
{
    public class DeleteReservationViewModel
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Models.Car? Car { get; set; }
    }
}
