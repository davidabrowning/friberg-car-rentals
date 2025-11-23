using FribergCarRentals.Core.Models;

namespace FribergCarRentals.WebApi.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string HomeCity { get; set; } = string.Empty;
        public string HomeCountry { get; set; } = string.Empty;
    }
}
