using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.Mvc.ApiClients;
using FribergCarRentals.Mvc.Session;
using FribergCarRentals.WebApi.Dtos;
using Polly;

namespace FribergCarRentals.Mvc
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add web services to the container
            builder.Services.AddHttpClient<ICRUDApiClient<CarDto>, CarApiClient>(client => client.BaseAddress = new Uri("https://localhost:7175"));
            builder.Services.AddHttpClient<ICRUDApiClient<AdminDto>, AdminApiClient>(client => client.BaseAddress = new Uri("https://localhost:7175"));
            builder.Services.AddHttpClient<ICRUDApiClient<CustomerDto>, CustomerApiClient>(client => client.BaseAddress = new Uri("https://localhost:7175"));
            builder.Services.AddHttpClient<ICRUDApiClient<ReservationDto>, ReservationApiClient>(client => client.BaseAddress = new Uri("https://localhost:7175"));
            builder.Services.AddHttpClient<IUserApiClient, UserApiClient>(client => client.BaseAddress = new Uri("https://localhost:7175"))
                .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, retry => TimeSpan.FromMilliseconds(200)));

            // Add other services to the container
            builder.Services.AddControllersWithViews();

            // Add a tracker for login status
            builder.Services.AddSingleton<UserSession, UserSession>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
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

            app.Run();
        }
    }
}
