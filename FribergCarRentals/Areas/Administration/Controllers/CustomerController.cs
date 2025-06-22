using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Interfaces;
using FribergCarRentals.Areas.Administration.Views.Customer;

namespace FribergCarRentals.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administration")]
    public class CustomerController : Controller
    {
        private readonly IUserService _userService;

        public CustomerController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: Customer
        public IActionResult Index()
        {
            return RedirectToAction("Index", "IdentityUser");
        }

        // GET: Customer/Create/abcd-1234
        [HttpGet("Administration/Customer/Create/{identityUserId}")]
        public async Task<IActionResult> Create(string? identityUserId)
        {
            if (identityUserId == null)
            {
                return NotFound();
            }

            IdentityUser? identityUser = await _userService.GetUserById(identityUserId);
            if (identityUser == null)
            {
                return NotFound();
            }

            CreateCustomerViewModel createCustomerViewModel = new()
            {
                IdentityUserId = identityUser.Id,
                IdentityUserUsername = identityUser.UserName ?? "Unable to get username",
            };
            return View(createCustomerViewModel);
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerViewModel createCustomerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createCustomerViewModel);
            }

            IdentityUser? identityUser = await _userService.GetUserById(createCustomerViewModel.IdentityUserId);
            if (identityUser == null)
            {
                return NotFound();
            }

            Customer customer = new()
            {
                IdentityUser = identityUser,
                FirstName = createCustomerViewModel.FirstName,
                LastName = createCustomerViewModel.LastName,
                HomeCity = createCustomerViewModel.HomeCity,
                HomeCountry = createCustomerViewModel.HomeCountry,
                Reservations = new List<Reservation>(),
            };
            await _userService.CreateCustomerAsync(customer);

            return RedirectToAction(nameof(Index));
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

            EditCustomerViewModel editCustomerViewModel = new EditCustomerViewModel()
            {
                CustomerId = customer.Id,
                IdentityUserId = customer.IdentityUser.Id,
                IdentityUserUsername = customer.IdentityUser.UserName ?? "Unable to fetch username",
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                HomeCity = customer.HomeCity,
                HomeCountry = customer.HomeCountry,
                ReservationIds = customer.Reservations.Select(c => c.Id).ToList(),
            };

            return View(editCustomerViewModel);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCustomerViewModel editCustomerViewModel)
        {
            if (id != editCustomerViewModel.CustomerId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(editCustomerViewModel);
            }

            Customer? customer = await _userService.GetCustomerByIdAsync(editCustomerViewModel.CustomerId);

            if (customer == null)
            {
                return NotFound();
            }

            customer.FirstName = editCustomerViewModel.FirstName;
            customer.LastName = editCustomerViewModel.LastName;
            customer.HomeCity = editCustomerViewModel.HomeCity;
            customer.HomeCountry = editCustomerViewModel.HomeCountry;
            await _userService.UpdateCustomerAsync(customer);

            return RedirectToAction("Index");
        }

        // GET: Customer/DeleteAsync/5
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

            DeleteCustomerViewModel deleteCustomerViewModel = new DeleteCustomerViewModel()
            {
                CustomerId = customer.Id,
                IdentityUserId = customer.IdentityUser.Id,
                IdentityUserUsername = customer.IdentityUser.UserName ?? "Unable to fetch username",
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                HomeCity = customer.HomeCity,
                HomeCountry = customer.HomeCountry,
                ReservationIds = customer.Reservations.Select(c => c.Id).ToList(),
            };

            return View(deleteCustomerViewModel);
        }

        // POST: Customer/DeleteAsync/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _userService.DeleteCustomerAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
