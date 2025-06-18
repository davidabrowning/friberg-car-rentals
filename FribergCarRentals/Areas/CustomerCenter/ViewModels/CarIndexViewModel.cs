namespace FribergCarRentals.Areas.CustomerCenter.ViewModels
{
    public class CarIndexViewModel
    {
        public int Id { get; set; }
        public string Make { get; set; } = "";
        public string Model { get; set; } = "";
        public int Year { get; set; } = 0;
        public string Description { get; set; } = "";
    }
}
