namespace FribergCarRentals.Mvc.Areas.Administration.Views.Customer
{
    public class EditCustomerViewModel
    {
        public int CustomerId { get; set; }
        public string UserId { get; set; } = "";
        public string Username { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string HomeCity { get; set; } = "";
        public string HomeCountry { get; set; } = "";
        public List<int> ReservationIds { get; set; } = new();
    }
}
