using FribergCarRentals.Core.Models;

namespace FribergCarRentals.WebApi.Dtos
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public CarDto CarDto { get; set; } = new();
        public CustomerDto CustomerDto { get; set; } = new();
    }
}
