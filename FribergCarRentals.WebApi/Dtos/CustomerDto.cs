using FribergCarRentals.Core.Models;

namespace FribergCarRentals.WebApi.Dtos
{
    public class CustomerDto
    {
        public required int Id { get; set; }
        public required string UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string HomeCity { get; set; }
        public required string HomeCountry { get; set; }
    }
}
