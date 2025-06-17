using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Areas.Public.ViewModels;
using FribergCarRentals.ViewModels;
using System.Diagnostics;
using FribergCarRentals.Interfaces;

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
            HomeIndexViewModel homeIndexViewModel = new();

            IdentityUser? identityUser = await _userService.GetCurrentSignedInIdentityUserAsync();
            if (identityUser == null)
            {
                homeIndexViewModel.IsSignedIn = false;
                homeIndexViewModel.HasCustomerAccount = false;
            }
            else
            {
                IEnumerable<Customer> customers = await _userService.GetAllCustomersAsync();
                homeIndexViewModel.IsSignedIn = true;
                homeIndexViewModel.HasCustomerAccount = customers.Any(c => c.IdentityUser == identityUser);
            }
            return View(homeIndexViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
