namespace FribergCarRentals.Areas.Administration.ViewModels
{
    public class CustomerCreateViewModel
    {
        public string IdentityUserId { get; set; } = "";
        public string IdentityUserUsername { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string HomeCity { get; set; } = "";
        public string HomeCountry { get; set; } = "";
    }
}
