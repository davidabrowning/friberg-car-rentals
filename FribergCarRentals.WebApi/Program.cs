
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
        private static WebApplicationBuilder? _builder;
        private static WebApplication? _app;
        public static async Task Main(string[] args)
        {
            _builder = WebApplication.CreateBuilder(args);
            PrepDependencyInjection();
            _app = _builder.Build();
            await PrepDatabase();
            ConfigureMiddleware();
            ConfigureEndpoints();
            _app.Run();
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
            string connectionString = _builder!.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            _builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            _builder.Services.AddScoped<IDatabaseCleaner, DatabaseCleaningService>();
            _builder.Services.AddScoped<IDatabaseSeeder, DatabaseSeedingService>();
        }

        private static void AddRepository()
        {
            _builder!.Services.AddScoped<IRepository<Admin>, AdminRepository>();
            _builder.Services.AddScoped<IRepository<Car>, CarRepository>();
            _builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();
            _builder.Services.AddScoped<IRepository<Reservation>, ReservationRepository>();
        }

        private static void AddServices()
        {
            _builder!.Services.AddScoped<IAdminService, AdminService>();
            _builder.Services.AddScoped<IAuthService, AuthService>();
            _builder.Services.AddScoped<ICarService, CarService>();
            _builder.Services.AddScoped<ICustomerService, CustomerService>();
            _builder.Services.AddScoped<IReservationService, ReservationService>();
            _builder.Services.AddScoped<IApplicationFacade, ApplicationFacade>();
        }

        private static void AddIdentity()
        {
            _builder!.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
        }

        private static void AddJwt()
        {
            _builder!.Services.AddAuthentication("JwtBearer")
                .AddJwtBearer("JwtBearer", options =>
                    {
                        options.TokenValidationParameters = new()
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateIssuerSigningKey = true,
                            ValidateLifetime = true,
                            ValidIssuer = _builder.Configuration["Jwt:Issuer"],
                            ValidAudience = _builder.Configuration["Jwt:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(_builder.Configuration["Jwt:Key"]!)
                                )
                        };
                    });
            _builder.Services.AddScoped<IJwtService, JwtService>();
        }

        private static void AddControllers()
        {
            _builder!.Services.AddControllers();
        }

        private static void AddOpenApi()
        {
            _builder!.Services.AddOpenApi();
        }

        private async static Task PrepDatabase()
        {
            await SeedDatabase();
            await CleanDatabase();
        }

        private async static Task SeedDatabase()
        {
            using (IServiceScope scope = _app.Services.CreateScope())
            {
                IDatabaseSeeder seedingService = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();
                await seedingService.SeedAsync();
            }
        }

        private async static Task CleanDatabase()
        {
            using (IServiceScope scope = _app.Services.CreateScope())
            {
                IDatabaseCleaner cleaningService = scope.ServiceProvider.GetRequiredService<IDatabaseCleaner>();
                await cleaningService.CleanAsync();
            }
        }

        private static void ConfigureMiddleware()
        {
            if (_app!.Environment.IsDevelopment())
            {
                _app.MapOpenApi();
                _app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "FribergCarRentalsApi"));
            }
            _app.UseHttpsRedirection();
            _app.UseAuthentication();
            _app.UseAuthorization();
        }

        private static void ConfigureEndpoints()
        {
            _app!.MapControllers();
        }
    }
}
