using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Core.Models;
using FribergCarRentals.ViewModels;
using System.Diagnostics;
using FribergCarRentals.Areas.Public.Views.Home;
using FribergCarRentals.Core.Interfaces.Services;

namespace FribergCarRentals.Areas.Public.Controllers
{
    [Area("Public")]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: Public/Home
        public async Task<IActionResult> Index()
        {
            IndexHomeViewModel homeIndexViewModel = new();

            string? userId = await _userService.GetCurrentUserId();
            if (userId == null)
            {
                homeIndexViewModel.IsSignedIn = false;
                homeIndexViewModel.HasCustomerAccount = false;
                return View(homeIndexViewModel);
            }

            Customer? customer = await _userService.GetCustomerByUserIdAsync(userId);
            if (customer == null)
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
