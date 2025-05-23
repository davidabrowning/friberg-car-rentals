using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.ViewModels;

namespace FribergCarRentals.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Admin> Admins { get; set; }
        // Note that DbSet<ApplicationUser> is already included by default and doesn't need to be declared here.
        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<FribergCarRentals.ViewModels.ApplicationUserViewModel> ApplicationUserViewModel { get; set; } = default!;
    }
}
