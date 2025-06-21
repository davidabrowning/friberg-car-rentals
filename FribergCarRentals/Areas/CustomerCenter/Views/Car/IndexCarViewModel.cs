namespace FribergCarRentals.Areas.CustomerCenter.Views.Car
{
    public class IndexCarViewModel
    {
        public int Id { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; } = 0;
        public string Description { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
    }
}
