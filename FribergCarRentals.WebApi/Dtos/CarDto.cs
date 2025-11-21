namespace FribergCarRentals.WebApi.Dtos
{
    public class CarDto
    {
        public int Id { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<string> PhotoUrls { get; set; } = new();
    }
}
