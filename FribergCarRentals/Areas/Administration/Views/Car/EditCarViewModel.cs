﻿namespace FribergCarRentals.Areas.Administration.Views.Car
{
    public class EditCarViewModel
    {
        public int Id { get; set; }
        public string Make { get; set; } = "";
        public string Model { get; set; } = "";
        public int Year { get; set; } = 0;
        public string Description { get; set; } = "";
    }
}
