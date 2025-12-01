using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.Mvc.ApiClients;
using FribergCarRentals.Mvc.Session;
using FribergCarRentals.WebApi.Dtos;
using Polly;

namespace FribergCarRentals.Mvc
{
    public class Program
    {
        private static WebApplicationBuilder? _builder;
        private static WebApplication? _app;
        public static void Main(string[] args)
        {
            _builder = WebApplication.CreateBuilder(args);
            PrepDependencyInjection();
            _app = _builder.Build();
            ConfigureMiddleware();
            ConfigureEndpoints();
            _app.Run();
        }

        private static void PrepDependencyInjection()
        {
            AddApiClient();
            AddControllers();
            AddUserSession();
        }

        private static void AddApiClient()
        {
            _builder!.Services.AddHttpClient<ICRUDApiClient<CarDto>, CarApiClient>(client => client.BaseAddress = new Uri("https://localhost:7175"));
            _builder.Services.AddHttpClient<ICRUDApiClient<AdminDto>, AdminApiClient>(client => client.BaseAddress = new Uri("https://localhost:7175"));
            _builder.Services.AddHttpClient<ICRUDApiClient<CustomerDto>, CustomerApiClient>(client => client.BaseAddress = new Uri("https://localhost:7175"));
            _builder.Services.AddHttpClient<ICRUDApiClient<ReservationDto>, ReservationApiClient>(client => client.BaseAddress = new Uri("https://localhost:7175"));
            _builder.Services.AddHttpClient<IUserApiClient, UserApiClient>(client => client.BaseAddress = new Uri("https://localhost:7175"))
                .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, retry => TimeSpan.FromMilliseconds(200)));
        }

        private static void AddControllers()
        {
            _builder!.Services.AddControllersWithViews();
        }

        private static void AddUserSession()
        {
            _builder!.Services.AddSingleton<UserSession, UserSession>();
        }

        private static void ConfigureMiddleware()
        {
            if (_app!.Environment.IsDevelopment())
            {
            }
            else
            {
                _app.UseExceptionHandler("/Public/Home/Error");
                _app.UseHsts();
            }
            _app.UseHttpsRedirection();
            _app.UseRouting();
            _app.UseAuthorization();
            _app.MapStaticAssets();
        }

        private static void ConfigureEndpoints()
        {
            _app!.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            _app!.MapControllerRoute(
                name: "default",
                pattern: "{area=Public}/{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
        }
    }
}
