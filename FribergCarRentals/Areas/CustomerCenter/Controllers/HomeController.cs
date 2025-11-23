using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Mvc.Areas.CustomerCenter.Controllers
{
    [Authorize(Roles = "Customer")]
    [Area("CustomerCenter")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
