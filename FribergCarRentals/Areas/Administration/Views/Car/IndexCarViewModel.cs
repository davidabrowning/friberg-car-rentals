using FribergCarRentals.Core.Models;

namespace FribergCarRentals.Mvc.Areas.Administration.Views.Car
{
    public class IndexCarViewModel
    {
        public int Id { get; set; }
        public string Make { get; set; } = "";
        public string Model { get; set; } = "";
        public int Year { get; set; } = 0;
        public string Description { get; set; } = "";
        public List<int> ReservationIds { get; set; } = new();
    }
}
