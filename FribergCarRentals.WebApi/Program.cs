
using FribergCarRentals.Core.Interfaces.Other;
using FribergCarRentals.Core.Interfaces.Repositories;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;
using FribergCarRentals.Data;
using FribergCarRentals.WebApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FribergCarRentals.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddScoped<IRepository<Admin>, AdminRepository>();
            builder.Services.AddScoped<IRepository<Car>, CarRepository>();
            builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();
            builder.Services.AddScoped<IRepository<Reservation>, ReservationRepository>();
            builder.Services.AddScoped<IBasicCRUDService<Admin>>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ICarService, CarService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IReservationService, ReservationService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IDatabaseCleaner, DatabaseCleaningService>();
            builder.Services.AddScoped<IDatabaseSeeder, DatabaseSeedingService>();

            // Identity login
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // JWT login
            builder.Services.AddAuthentication("JwtBearer")
                .AddJwtBearer("JwtBearer", options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:KEy"]!)
                            )
                    };
                });
            builder.Services.AddScoped<IJwtService, JwtService>();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

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
                app.MapOpenApi();
                app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "FribergCarRentalsApi"));
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
