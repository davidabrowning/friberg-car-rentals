using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Areas.Administration.ViewModels;
using FribergCarRentals.Interfaces;

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

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
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

            CustomerDetailsViewModel customerDetailsViewModel = new CustomerDetailsViewModel()
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

            return View(customerDetailsViewModel);
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

            CustomerCreateViewModel customerCreateViewModel = new()
            {
                IdentityUserId = identityUser.Id,
                IdentityUserUsername = identityUser.UserName,
            };
            return View(customerCreateViewModel);
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerCreateViewModel customerCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(customerCreateViewModel);
            }

            IdentityUser? identityUser = await _userService.GetUserById(customerCreateViewModel.IdentityUserId);
            if (identityUser == null)
            {
                return NotFound();
            }

            Customer customer = new()
            {
                IdentityUser = identityUser,
                FirstName = customerCreateViewModel.FirstName,
                LastName = customerCreateViewModel.LastName,
                HomeCity = customerCreateViewModel.HomeCity,
                HomeCountry = customerCreateViewModel.HomeCountry,
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

            CustomerEditViewModel customerEditViewModel = new CustomerEditViewModel()
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

            Customer? customer = await _userService.GetCustomerByIdAsync(customerEditViewModel.CustomerId);

            if (customer == null)
            {
                return NotFound();
            }

            customer.FirstName = customerEditViewModel.FirstName;
            customer.LastName = customerEditViewModel.LastName;
            customer.HomeCity = customerEditViewModel.HomeCity;
            customer.HomeCountry = customerEditViewModel.HomeCountry;
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

            CustomerDeleteViewModel customerDeleteViewModel = new CustomerDeleteViewModel()
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

            return View(customerDeleteViewModel);
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
