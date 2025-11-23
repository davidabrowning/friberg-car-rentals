using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.Mvc.Areas.Administration.Views.Reservation
{
    public class IndexReservationViewModel
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public CarDto CarDto { get; set; } = new();
        public CustomerDto CustomerDto { get; set; } = new();
    }
}
