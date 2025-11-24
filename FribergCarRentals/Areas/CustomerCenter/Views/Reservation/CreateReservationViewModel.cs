using FribergCarRentals.WebApi.Dtos;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Mvc.Areas.CustomerCenter.Views.Reservation
{
    public class CreateReservationViewModel : IValidatableObject
    {
        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Today).AddDays(1);
        public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Today).AddDays(8);
        public int CarId { get; set; }
        public int CustomerId { get; set; }
        public int PreselectedCarId { get; set; }
        public IEnumerable<CarDto> CarDtos { get; set; } = new List<CarDto>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate < DateOnly.FromDateTime(DateTime.Today))
            {
                yield return new ValidationResult("Start date cannot be in the past.", new List<string>() { "StartDate" });
            }
            if (EndDate < StartDate)
            {
                yield return new ValidationResult("End date must be on or after start date.", new List<string>() { "EndDate" } );
            }
        }
    }
}
