﻿namespace FribergCarRentals.Areas.Administration.Views.IdentityUser
{
    public class CreateIdentityUserViewModel
    {
        public string Id { get; set; } = "";
        public string Username { get; set; } = "";
        public bool IsAdmin { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsUser { get; set; }
        public int AdminId { get; set; }
        public string AdminFirstName { get; set; } = "";
        public string AdminLastName { get; set; } = "";
        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; } = "";
        public string CustomerLastName { get; set; } = "";
    }
}
