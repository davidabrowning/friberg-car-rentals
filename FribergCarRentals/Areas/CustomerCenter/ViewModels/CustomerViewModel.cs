namespace FribergCarRentals.Areas.CustomerCenter.ViewModels
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        public string IdentityUserId { get; set; } = "";
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string HomeCity { get; set; } = "";
        public string HomeCountry { get; set; } = "";
        public List<int> ReservationIds { get; set; } = new();
    }
}
