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
using FribergCarRentals.Interfaces;
using FribergCarRentals.Areas.CustomerCenter.Views.Customer;

namespace FribergCarRentals.Areas.CustomerCenter.Controllers
{
    [Authorize(Roles = "Customer")]
    [Area("CustomerCenter")]
    public class CustomerController : Controller
    {
        private readonly IUserService _userService;
        public CustomerController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: CustomerCenter/Customer/Details
        public async Task<IActionResult> Details()
        {
            Customer? customer = await _userService.GetSignedInCustomer();
            if (customer == null)
            {
                return NotFound();
            }

            DetailsCustomerViewModel detailsCustomerViewModel = new()
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                HomeCity = customer.HomeCity,
                HomeCountry = customer.HomeCountry,
            };

            return View(detailsCustomerViewModel);
        }

        // GET: CustomerCenter/Customer/Edit
        public async Task<IActionResult> Edit()
        {
            Customer? customer = await _userService.GetSignedInCustomer();
            if (customer == null)
            {
                return NotFound();
            }

            EditCustomerViewModel editCustomerViewModel = new()
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                HomeCity = customer.HomeCity,
                HomeCountry = customer.HomeCountry,
            };

            return View(editCustomerViewModel);
        }

        // POST: CustomerCenter/Customer/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCustomerViewModel editCustomerViewModel)
        {
            Customer? customer = await _userService.GetSignedInCustomer();
            if (customer == null)
            {
                return NotFound();
            }

            if (customer.Id != editCustomerViewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(editCustomerViewModel);
            }

            customer.FirstName = editCustomerViewModel.FirstName;
            customer.LastName = editCustomerViewModel.LastName;
            customer.HomeCity = editCustomerViewModel.HomeCity;
            customer.HomeCountry = editCustomerViewModel.HomeCountry;
            await _userService.UpdateCustomerAsync(customer);

            return RedirectToAction("Details");
        }
    }
}
