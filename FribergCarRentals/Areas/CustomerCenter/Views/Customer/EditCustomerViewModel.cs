﻿namespace FribergCarRentals.Areas.CustomerCenter.Views.Customer
{
    public class EditCustomerViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string HomeCity { get; set; } = "";
        public string HomeCountry { get; set; } = "";
    }
}
