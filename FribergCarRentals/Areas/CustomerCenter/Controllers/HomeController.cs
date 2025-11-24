using FribergCarRentals.Mvc.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Mvc.Areas.CustomerCenter.Controllers
{
    [RequireCustomer]
    [Area("CustomerCenter")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
