namespace FribergCarRentals.Mvc.Areas.Administration.Views.Car
{
    public class CreateCarViewModel
    {
        public string Make { get; set; } = "";
        public string Model { get; set; } = "";
        public int Year { get; set; } = DateTime.Now.Year;
        public string Description { get; set; } = "";
    }
}
