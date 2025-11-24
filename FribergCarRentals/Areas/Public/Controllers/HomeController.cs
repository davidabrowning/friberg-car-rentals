using FribergCarRentals.Mvc.Areas.Public.Views.Home;
using FribergCarRentals.Mvc.Session;
using FribergCarRentals.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FribergCarRentals.Mvc.Areas.Public.Controllers
{
    [Area("Public")]
    public class HomeController : Controller
    {
        private readonly UserSession _userSession;
        public HomeController(UserSession userSession)
        {
            _userSession = userSession;
        }

        // GET: Public/Home
        public async Task<IActionResult> Index()
        {
            IndexHomeViewModel homeIndexViewModel = new();

            if (!_userSession.IsSignedIn())
            {
                homeIndexViewModel.IsSignedIn = false;
                homeIndexViewModel.HasCustomerAccount = false;
                return View(homeIndexViewModel);
            }

            if (!_userSession.IsCustomer())
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
