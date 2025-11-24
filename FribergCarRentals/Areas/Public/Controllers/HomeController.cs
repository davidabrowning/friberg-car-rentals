using FribergCarRentals.Mvc.Areas.Public.Views.Home;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.Mvc.Areas.Public.Controllers
{
    [Area("Public")]
    public class HomeController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        public HomeController(IUserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }

        // GET: Public/Home
        public async Task<IActionResult> Index()
        {
            IndexHomeViewModel homeIndexViewModel = new();

            UserDto userDto = await _userApiClient.GetCurrentUserAsync();
            if (userDto.UserId == null)
            {
                homeIndexViewModel.IsSignedIn = false;
                homeIndexViewModel.HasCustomerAccount = false;
                return View(homeIndexViewModel);
            }

            if (userDto.CustomerDto == null)
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
