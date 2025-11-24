namespace FribergCarRentals.Mvc.Areas.Administration.Views.ApplicationUser
{
    public class IndexApplicationUserViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public int AdminId { get; set; }
        public string AdminName { get; set; } = string.Empty;
        public bool IsCustomer { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
    }
}
