using FribergCarRentals.Mvc.Areas.Public.Views.Home;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FribergCarRentals.Mvc.Areas.Public.Controllers
{
    [Area("Public")]
    public class HomeController : Controller
    {
        private readonly IAuthApiClient _authApiClient;
        public HomeController(IAuthApiClient authApiClient)
        {
            _authApiClient = authApiClient;
        }

        // GET: Public/Home
        public async Task<IActionResult> Index()
        {
            IndexHomeViewModel homeIndexViewModel = new();

            string? userId = await _authApiClient.GetCurrentSignedInUserIdAsync();
            if (userId == null)
            {
                homeIndexViewModel.IsSignedIn = false;
                homeIndexViewModel.HasCustomerAccount = false;
                return View(homeIndexViewModel);
            }

            bool isCustomer = await _authApiClient.IsCustomerAsync(userId);
            if (!isCustomer)
            {
                homeIndexViewModel.IsSignedIn = true;
                homeIndexViewModel.HasCustomerAccount = false;
                return View(homeIndexViewModel);
            }

            return RedirectToAction("Index", "Home", new { area = "CustomerCenter" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
