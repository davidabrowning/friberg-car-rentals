using FribergCarRentals.Core.Models;

namespace FribergCarRentals.WebApi.Dtos
{
    public class ReservationDto
    {
        public required int Id { get; set; }
        public required DateOnly StartDate { get; set; }
        public required DateOnly EndDate { get; set; }
        public required CarDto CarDto { get; set; }
        public required CustomerDto CustomerDto { get; set; }
    }
}
