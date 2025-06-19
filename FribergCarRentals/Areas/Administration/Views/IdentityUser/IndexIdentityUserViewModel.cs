namespace FribergCarRentals.Areas.Administration.Views.IdentityUser
{
    public class IndexIdentityUserViewModel
    {
        public string IdentityUserId { get; set; }
        public string IdentityUserUsername { get; set; }
        public bool IsAdmin { get; set; }
        public int AdminId { get; set; }
        public string AdminName { get; set; }
        public bool IsCustomer { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
}
