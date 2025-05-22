using FribergCarRentals.Models;

namespace FribergCarRentals.ViewModels
{
    public class CustomerIndexViewModel
    {
        public List<ApplicationUser> ApplicationUsers { get; set; }
        public List<Customer> Customers { get; set; }
    }
}
