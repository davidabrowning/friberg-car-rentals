using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Areas.Public.ViewModels;
using FribergCarRentals.Interfaces;

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
        public IActionResult Create()
        {
            CustomerCreateViewModel emptyVM = new();
            return View(emptyVM);
        }

        // POST: Public/Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerCreateViewModel populatedCustomerCreateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(populatedCustomerCreateVM);
            }

            IdentityUser? identityUser = await _userService.GetCurrentUser();
            if (identityUser == null)
            {
                return NotFound();
            }

            Customer newCustomer = new Customer()
            {
                IdentityUser = identityUser,
                FirstName = populatedCustomerCreateVM.FirstName,
                LastName = populatedCustomerCreateVM.LastName,
                HomeCity = populatedCustomerCreateVM.HomeCity,
                HomeCountry = populatedCustomerCreateVM.HomeCountry,
            };

            await _userService.CreateCustomerAsync(newCustomer);

            return RedirectToAction(nameof(Index));
        }
    }
}
