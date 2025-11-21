namespace FribergCarRentals.WebApi.Dtos
{
    public class CarDto
    {
        public required int Id { get; set; }
        public required string Make { get; set; }
        public required string Model { get; set; }
        public required int Year { get; set; }
        public required string Description { get; set; }
        public required List<string> PhotoUrls { get; set; }
    }
}
