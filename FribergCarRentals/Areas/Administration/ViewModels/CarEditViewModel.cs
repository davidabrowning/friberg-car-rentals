namespace FribergCarRentals.Areas.Administration.ViewModels
{
    public class CarEditViewModel
    {
        public int Id { get; set; }
        public string Make { get; set; } = "";
        public string Model { get; set; } = "";
        public int Year { get; set; } = 0;
        public string Description { get; set; } = "";
    }
}
