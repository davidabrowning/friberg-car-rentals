using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Core.Interfaces;
using FribergCarRentals.Areas.Public.Views.Customer;
using FribergCarRentals.Helpers;

namespace FribergCarRentals.Areas.Public.Controllers
{
    [Authorize]
    [Area("Public")]
    public class CustomerController : Controller
    {
        private readonly IUserService _userService;

        public CustomerController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: Public/Customer
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        // GET: Public/Customer/Create
        public async Task<IActionResult> Create()
        {
            Customer? signedInCustomer = await _userService.GetSignedInCustomer();
            if (signedInCustomer != null)
            {
                return RedirectToAction("Index", "Car", new { area = "CustomerCenter" });
            }

            CreateCustomerViewModel emptyVM = new();
            return View(emptyVM);
        }

        // POST: Public/Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerViewModel populatedCustomerCreateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(populatedCustomerCreateVM);
            }

            string? userId = await _userService.GetCurrentUserId();
            if (userId == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

            Customer newCustomer = new Customer()
            {
                UserId = userId,
                FirstName = populatedCustomerCreateVM.FirstName,
                LastName = populatedCustomerCreateVM.LastName,
                HomeCity = populatedCustomerCreateVM.HomeCity,
                HomeCountry = populatedCustomerCreateVM.HomeCountry,
            };

            await _userService.CreateCustomerAsync(newCustomer);

            TempData["SuccessMessage"] = UserMessage.SuccessCustomerCreated;
            return RedirectToAction("Index");
        }
    }
}
