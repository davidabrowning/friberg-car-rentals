namespace FribergCarRentals.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; } = "";
        public ApplicationUser ApplicationUser { get; set; } = null;
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }
}
