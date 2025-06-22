using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Interfaces;
using FribergCarRentals.Areas.Administration.Views.Customer;
using FribergCarRentals.Helpers;

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
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsNull;
                return RedirectToAction("Index");
            }

            IdentityUser? identityUser = await _userService.GetUserById(identityUserId);
            if (identityUser == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
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

            TempData["SuccessMessage"] = UserMessage.SuccessCustomerCreated;
            return RedirectToAction("Index");
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsNull;
                return RedirectToAction("Index");
            }

            Customer? customer = await _userService.GetCustomerByIdAsync((int)id);
            if (customer == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
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
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsNull;
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(editCustomerViewModel);
            }

            Customer? customer = await _userService.GetCustomerByIdAsync(editCustomerViewModel.CustomerId);

            if (customer == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
            }

            customer.FirstName = editCustomerViewModel.FirstName;
            customer.LastName = editCustomerViewModel.LastName;
            customer.HomeCity = editCustomerViewModel.HomeCity;
            customer.HomeCountry = editCustomerViewModel.HomeCountry;
            await _userService.UpdateCustomerAsync(customer);

            TempData["SuccessMessage"] = UserMessage.SuccessCustomerUpdated;
            return RedirectToAction("Index");
        }

        // GET: Customer/DeleteAsync/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsNull;
                return RedirectToAction("Index");
            }

            Customer? customer = await _userService.GetCustomerByIdAsync((int)id);
            if (customer == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
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

            TempData["SuccessMessage"] = UserMessage.SuccessCustomerDeleted;
            return RedirectToAction("Index");
        }
    }
}
