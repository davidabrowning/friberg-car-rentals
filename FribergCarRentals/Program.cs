using FribergCarRentals.Data;
using FribergCarRentals.Interfaces;
using FribergCarRentals.Models;
using FribergCarRentals.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentals
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddScoped<IIdentityUserService, IdentityUserService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ICarService, CarService>();
            // builder.Services.AddScoped<IReservationService, ReservationService>();
            builder.Services.AddScoped<IRepository<Admin>, AdminRepository>();
            builder.Services.AddScoped<IRepository<Car>, CarRepository>();
            builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();
            builder.Services.AddScoped<DatabaseSeedingService, DatabaseSeedingService>();
            builder.Services.AddScoped<DatabaseCleaningService, DatabaseCleaningService>();
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Seed roles
            using (IServiceScope scope = app.Services.CreateScope())
            {
                DatabaseSeedingService seedingService = scope.ServiceProvider.GetRequiredService<DatabaseSeedingService>();
                await seedingService.Go();
            }

            // Run DB cleanup
            using (IServiceScope scope = app.Services.CreateScope())
            {
                DatabaseCleaningService cleaningService = scope.ServiceProvider.GetRequiredService<DatabaseCleaningService>();
                await cleaningService.Go();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Public/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Public}/{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
