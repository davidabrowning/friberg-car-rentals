using FribergCarRentals.Models;

namespace FribergCarRentals.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }
}
