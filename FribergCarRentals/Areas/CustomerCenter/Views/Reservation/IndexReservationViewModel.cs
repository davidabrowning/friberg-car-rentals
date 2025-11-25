using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.Mvc.Areas.CustomerCenter.Views.Reservation
{
    public class IndexReservationViewModel
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public CarDto CarDto { get; set; }
    }
}
