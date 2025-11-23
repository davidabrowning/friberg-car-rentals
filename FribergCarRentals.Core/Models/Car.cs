namespace FribergCarRentals.Core.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; } = "";
        public string Model { get; set; } = "";
        public int Year { get; set; } = 0;
        public string Description { get; set; } = "";
        public List<string> PhotoUrls { get; set; } = new();
        public List<Reservation> Reservations { get; set; } = new();

        public override bool Equals(object? obj)
        {
            return obj is Car car &&
                   Id == car.Id;
        }
    }
}
