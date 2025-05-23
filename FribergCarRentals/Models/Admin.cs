namespace FribergCarRentals.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }
}
