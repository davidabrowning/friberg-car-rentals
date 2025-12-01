using FribergCarRentals.WebApi.Dtos;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Mvc.Areas.Administration.Views.Car
{
    public class PhotosCarViewModel
    {
        public int Id { get; set; }
        public CarDto CarDto { get; set; } = new();
        [Url]
        public string? PhotoUrl1 { get; set; }
        [Url]
        public string? PhotoUrl2 { get; set; }
        [Url]
        public string? PhotoUrl3 { get; set; }
    }
}
