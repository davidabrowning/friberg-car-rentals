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

        // GET: CustomerCenter/Customer
        public async Task<IActionResult> Index()
        {
            IEnumerable<Customer> customers = await _userService.GetAllCustomersAsync();
            return View(customers);
        }

        // GET: CustomerCenter/Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _userService.GetCustomerByIdAsync((int)id);
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

            IdentityUser? identityUser = await _userService.GetCurrentSignedInIdentityUserAsync();
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

        // GET: CustomerCenter/Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _userService.GetCustomerByIdAsync((int)id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: CustomerCenter/Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,HomeCity,HomeCountry")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _userService.UpdateCustomerAsync(customer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _userService.CustomerIdExistsAsync(customer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: CustomerCenter/Customer/DeleteAsync/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer? customer = await _userService.GetCustomerByIdAsync((int)id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: CustomerCenter/Customer/DeleteAsync/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _userService.DeleteCustomerAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
