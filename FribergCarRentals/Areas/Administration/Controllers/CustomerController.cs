using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.Data;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Areas.Administration.ViewModels;
using FribergCarRentals.Areas.Administration.Helpers;
using FribergCarRentals.Interfaces;

namespace FribergCarRentals.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administration")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IIdentityUserService _identityUserService;

        public CustomerController(ICustomerService customerService, IIdentityUserService identityUserService)
        {
            _customerService = customerService;
            _identityUserService = identityUserService;
        }

        // GET: Customer
        public IActionResult Index()
        {
            return RedirectToAction("Index", "IdentityUser");
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer? customer = await _customerService.GetByIdAsync((int)id);
            if (customer == null)
            {
                return NotFound();
            }

            CustomerEditViewModel customerViewModel = ViewModelMakerHelper.MakeCustomerEditViewModel(customer);
            return View(customerViewModel);
        }

        // GET: Customer/Create/abcd-1234
        [HttpGet("Administration/Customer/Create/{identityUserId}")]
        public async Task<IActionResult> Create(string? identityUserId)
        {
            if (identityUserId == null)
            {
                return NotFound();
            }

            IdentityUser? identityUser = await _identityUserService.GetByIdAsync(identityUserId);
            if (identityUser == null)
            {
                return NotFound();
            }

            CustomerCreateViewModel customerCreateViewModel = new()
            {
                IdentityUserId = identityUser.Id,
                IdentityUserUsername = identityUser.UserName,
            };
            return View(customerCreateViewModel);
        }

        // POST: Customer/Create
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

            IdentityUser? identityUser = await _identityUserService.GetByIdAsync(customerCreateViewModel.IdentityUserId);
            if (identityUser == null)
            {
                return NotFound();
            }

            List<Reservation> reservations = new();
            Customer customer = ViewModelToCreateHelper.CreateNewCustomer(customerCreateViewModel, identityUser, reservations);
            await _customerService.AddAsync(customer);
            return RedirectToAction(nameof(Index));
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer? customer = await _customerService.GetByIdAsync((int)id);
            if (customer == null)
            {
                return NotFound();
            }

            CustomerEditViewModel customerEditViewModel = ViewModelMakerHelper.MakeCustomerEditViewModel(customer);
            return View(customerEditViewModel);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerEditViewModel customerEditViewModel)
        {
            if (id != customerEditViewModel.CustomerId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(customerEditViewModel);
            }

            try
            {
                Customer? customer = await _customerService.GetByIdAsync(customerEditViewModel.CustomerId);
                ViewModelToUpdateHelper.UpdatedExistingCustomer(customer, customerEditViewModel);
                await _customerService.UpdateAsync(customer);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _customerService.IdExistsAsync(customerEditViewModel.CustomerId))
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

        // GET: Customer/DeleteAsync/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer? customer = await _customerService.GetByIdAsync((int)id);
            if (customer == null)
            {
                return NotFound();
            }

            CustomerEditViewModel customerViewModel = ViewModelMakerHelper.MakeCustomerEditViewModel(customer);
            return View(customerViewModel);
        }

        // POST: Customer/DeleteAsync/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _customerService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
