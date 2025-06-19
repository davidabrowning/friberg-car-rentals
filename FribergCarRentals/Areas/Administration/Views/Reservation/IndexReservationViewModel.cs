using FribergCarRentals.Models;

namespace FribergCarRentals.Areas.Administration.Views.Reservation
{
    public class IndexReservationViewModel
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Models.Car Car { get; set; }
        public Models.Customer Customer { get; set; }
    }
}
