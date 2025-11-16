using FribergCarRentals.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace FribergCarRentals.Data
{
    public class ApplicationDbContextSeparated : IdentityDbContext<IdentityUser>
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        // Note that DbSet<IdentityUserId> is already included by default and doesn't need to be declared here.
        public DbSet<Reservation> Reservations { get; set; }
        public ApplicationDbContextSeparated(DbContextOptions<ApplicationDbContextSeparated> options) : base(options)
        {
        }    
    }
}
