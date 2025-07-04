﻿namespace FribergCarRentals.Areas.CustomerCenter.Views.Car
{
    public class DetailsCarViewModel
    {
        public int Id { get; set; }
        public string Make { get; set; } = "";
        public string Model { get; set; } = "";
        public int Year { get; set; } = 0;
        public string Description { get; set; } = "";
        public List<string> PhotoUrls { get; set; } = new();
    }
}
