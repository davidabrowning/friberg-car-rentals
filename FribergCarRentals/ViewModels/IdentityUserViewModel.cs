namespace FribergCarRentals.ViewModels
{
    public class IdentityUserViewModel
    {
        public string Id { get; set; } = "";
        public string Username { get; set; } = "";
        public bool IsAdmin { get; set; }
        public bool IsUser { get; set; }
    }
}
