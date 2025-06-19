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
using FribergCarRentals.Areas.CustomerCenter.ViewModels;
using FribergCarRentals.Interfaces;

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

            return View(customer);
        }

        // GET: CustomerCenter/Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CustomerCenter/Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerCreateViewModel customerCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(customerCreateViewModel);
            }

            IdentityUser? identityUser = await _userService.GetCurrentUser();
            Customer customer = new Customer()
            {
                IdentityUser = identityUser,
                FirstName = customerCreateViewModel.FirstName,
                LastName = customerCreateViewModel.LastName,
                HomeCity = customerCreateViewModel.HomeCity,
                HomeCountry = customerCreateViewModel.HomeCountry,
            };
            await _userService.CreateCustomerAsync(customer);
            return RedirectToAction(nameof(Index));
        }

        // GET: CustomerCenter/Customer/Edit
        public async Task<IActionResult> Edit()
        {
            Customer? customer = await _userService.GetSignedInCustomer();
            if (customer == null)
            {
                return NotFound();
            }

            CustomerEditViewModel customerEditViewModel = new()
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                HomeCity = customer.HomeCity,
                HomeCountry = customer.HomeCountry,
            };

            return View(customerEditViewModel);
        }

        // POST: CustomerCenter/Customer/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerEditViewModel customerEditViewModel)
        {
            Customer? customer = await _userService.GetSignedInCustomer();
            if (customer == null)
            {
                return NotFound();
            }

            if (customer.Id != customerEditViewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(customerEditViewModel);
            }

            customer.FirstName = customerEditViewModel.FirstName;
            customer.LastName = customerEditViewModel.LastName;
            customer.HomeCity = customerEditViewModel.HomeCity;
            customer.HomeCountry = customerEditViewModel.HomeCountry;
            await _userService.UpdateCustomerAsync(customer);

            return RedirectToAction("Details");
        }
    }
}
