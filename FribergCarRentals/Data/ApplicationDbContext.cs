using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Areas.CustomerCenter.ViewModels;

namespace FribergCarRentals.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        // Note that DbSet<IdentityUserId> is already included by default and doesn't need to be declared here.
        public DbSet<Reservation> Reservations { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<FribergCarRentals.Areas.CustomerCenter.ViewModels.CarDetailsViewModel> CarDetailViewModel { get; set; } = default!;
    }
}
