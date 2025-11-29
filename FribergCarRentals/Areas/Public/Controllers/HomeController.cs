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
        public IActionResult Index()
        {
            if (_userSession.IsCustomer())
                return RedirectToAction("Index", "Home", new { area = "CustomerCenter" });
            if (_userSession.IsSignedIn())
                return RedirectToAction("Create", "Customer");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
