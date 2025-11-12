using FribergCarRentals.Core.Models;

namespace FribergCarRentals.Areas.Administration.Views.Reservation
{
    public class DeleteReservationViewModel
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Core.Models.Car? Car { get; set; }
        public Core.Models.Customer? Customer { get; set; }
    }
}
