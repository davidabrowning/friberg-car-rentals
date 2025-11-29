
using FribergCarRentals.Core.Interfaces.Facades;
using FribergCarRentals.Core.Interfaces.Other;
using FribergCarRentals.Core.Interfaces.Repositories;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;
using FribergCarRentals.Data;
using FribergCarRentals.Services.Facades;
using FribergCarRentals.Services.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FribergCarRentals.WebApi
{
    public class Program
    {
        private static WebApplicationBuilder? builder;
        private static WebApplication? app;
        public static async Task Main(string[] args)
        {
            builder = WebApplication.CreateBuilder(args);
            PrepDependencyInjection();
            app = builder.Build();
            await PrepDatabase();
            ConfigureMiddleware();
            ConfigureEndpoints();
            app.Run();
        }

        private static void PrepDependencyInjection()
        {
            AddDatabase();
            AddRepository();
            AddServices();
            AddIdentity();
            AddJwt();
            AddControllers();
            AddOpenApi();
        }

        private static void AddDatabase()
        {
            string connectionString = builder!.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddScoped<IDatabaseCleaner, DatabaseCleaningService>();
            builder.Services.AddScoped<IDatabaseSeeder, DatabaseSeedingService>();
        }

        private static void AddRepository()
        {
            builder!.Services.AddScoped<IRepository<Admin>, AdminRepository>();
            builder.Services.AddScoped<IRepository<Car>, CarRepository>();
            builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();
            builder.Services.AddScoped<IRepository<Reservation>, ReservationRepository>();
        }

        private static void AddServices()
        {
            builder!.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ICarService, CarService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IReservationService, ReservationService>();
            builder.Services.AddScoped<IApplicationFacade, ApplicationFacade>();
        }

        private static void AddIdentity()
        {
            builder!.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
        }

        private static void AddJwt()
        {
            builder!.Services.AddAuthentication("JwtBearer")
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
                                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
                                )
                        };
                    });
            builder.Services.AddScoped<IJwtService, JwtService>();
        }

        private static void AddControllers()
        {
            builder!.Services.AddControllers();
        }

        private static void AddOpenApi()
        {
            builder!.Services.AddOpenApi();
        }

        private async static Task PrepDatabase()
        {
            await SeedDatabase();
            await CleanDatabase();
        }

        private async static Task SeedDatabase()
        {
            using (IServiceScope scope = app.Services.CreateScope())
            {
                IDatabaseSeeder seedingService = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();
                await seedingService.SeedAsync();
            }
        }

        private async static Task CleanDatabase()
        {
            using (IServiceScope scope = app.Services.CreateScope())
            {
                IDatabaseCleaner cleaningService = scope.ServiceProvider.GetRequiredService<IDatabaseCleaner>();
                await cleaningService.CleanAsync();
            }
        }

        private static void ConfigureMiddleware()
        {
            if (app!.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "FribergCarRentalsApi"));
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
        }

        private static void ConfigureEndpoints()
        {
            app!.MapControllers();
        }
    }
}
