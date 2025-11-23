using FribergCarRentals.Data;
using FribergCarRentals.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.WebApi.Services;
using FribergCarRentals.Core.Interfaces.Repositories;
using FribergCarRentals.Core.Interfaces.Other;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.Mvc.ApiClients;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.Mvc
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // Add data layer services to the container
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddScoped<IRepository<Admin>, AdminRepository>();
            builder.Services.AddScoped<IRepository<Car>, CarRepository>();
            builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();
            builder.Services.AddScoped<IRepository<Reservation>, ReservationRepository>();
            builder.Services.AddScoped<IDatabaseSeeder, DatabaseSeedingServiceSeparated>();
            builder.Services.AddScoped<IDatabaseCleaner, DatabaseCleaningServiceSeparated>();

            // Add service layer services to the container
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ICarService, CarService>();
            builder.Services.AddScoped<IReservationService, ReservationService>();

            // Add web services to the container
            builder.Services.AddHttpClient<ICRUDApiClient<CarDto>, CarApiClient>(client => client.BaseAddress = new Uri("https://localhost:7175"));
            builder.Services.AddHttpClient<ICRUDApiClient<AdminDto>, AdminApiClient>(client => client.BaseAddress = new Uri("https://localhost:7175"));
            builder.Services.AddHttpClient<ICRUDApiClient<CustomerDto>, CustomerApiClient>(client => client.BaseAddress = new Uri("https://localhost:7175"));
            builder.Services.AddHttpClient<ICRUDApiClient<ReservationDto>, ReservationApiClient>(client => client.BaseAddress = new Uri("https://localhost:7175"));
            builder.Services.AddHttpClient<IAuthApiClient, AuthApiClient>(client => client.BaseAddress = new Uri("https://localhost:7175"));

            // Add other services to the container
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Seed roles, default admin user, and default cars
            using (IServiceScope scope = app.Services.CreateScope())
            {
                IDatabaseSeeder seedingService = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();
                await seedingService.SeedAsync();
            }

            // Run DB cleanup
            using (IServiceScope scope = app.Services.CreateScope())
            {
                IDatabaseCleaner cleaningService = scope.ServiceProvider.GetRequiredService<IDatabaseCleaner>();
                await cleaningService.CleanAsync();
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
