using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Areas.Administration.Views.Car
{
    public class PhotosCarViewModel
    {
        public int Id { get; set; }
        public required Core.Models.Car Car { get; set; }
        [Url]
        public string? PhotoUrl1 { get; set; }
        [Url]
        public string? PhotoUrl2 { get; set; }
        [Url]
        public string? PhotoUrl3 { get; set; }
    }
}
