namespace FribergCarRentals.Areas.Administration.ViewModels
{
    public class CarCreateViewModel
    {
        public string Make { get; set; } = "";
        public string Model { get; set; } = "";
        public int Year { get; set; } = DateTime.Now.Year;
        public string Description { get; set; } = "";
    }
}
