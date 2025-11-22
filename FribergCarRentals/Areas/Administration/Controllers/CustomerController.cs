using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Core.Models;
using Microsoft.AspNetCore.Authorization;
using FribergCarRentals.Areas.Administration.Views.Customer;
using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administration")]
    public class CustomerController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICRUDApiClient<CustomerDto> _customerDtoApiClient;

        public CustomerController(IUserService userService, ICRUDApiClient<CustomerDto> customerDtoApiClient)
        {
            _userService = userService;
            _customerDtoApiClient = customerDtoApiClient;
        }

        // GET: Customer
        public IActionResult Index()
        {
            return RedirectToAction("Index", "IdentityUser");
        }

        // GET: Customer/Create/abcd-1234
        [HttpGet("Administration/Customer/Create/{userId}")]
        public async Task<IActionResult> Create(string? userId)
        {
            if (userId == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsNull;
                return RedirectToAction("Index");
            }

            string? username = await _userService.GetUsernameByUserId(userId);
            if (username == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

            CreateCustomerViewModel createCustomerViewModel = new()
            {
                IdentityUserId = userId,
                IdentityUserUsername = username,
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

            string userId = createCustomerViewModel.IdentityUserId;
            string? username = await _userService.GetUsernameByUserId(createCustomerViewModel.IdentityUserId);
            if (username == null)
            {
                return NotFound();
            }

            Customer customer = new()
            {
                UserId = userId,
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

            // Customer? customer = await _userService.GetCustomerByCustomerIdAsync((int)id);
            CustomerDto? customerDto = await _customerDtoApiClient.GetAsync((int)id);
            if (customerDto == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
            }

            string username = await _userService.GetUsernameByUserId(customerDto.UserId);
            EditCustomerViewModel editCustomerViewModel = new EditCustomerViewModel()
            {
                CustomerId = customerDto.Id,
                IdentityUserId = customerDto.UserId,
                IdentityUserUsername = username,
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                HomeCity = customerDto.HomeCity,
                HomeCountry = customerDto.HomeCountry,
                // ReservationIds = customerDto.Reservations.Select(c => c.Id).ToList(),
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

            Customer? customer = await _userService.GetCustomerByCustomerIdAsync(editCustomerViewModel.CustomerId);

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

            Customer? customer = await _userService.GetCustomerByCustomerIdAsync((int)id);
            if (customer == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
            }

            string username = await _userService.GetUsernameByUserId(customer.UserId);
            DeleteCustomerViewModel deleteCustomerViewModel = new DeleteCustomerViewModel()
            {
                CustomerId = customer.Id,
                IdentityUserId = customer.UserId,
                IdentityUserUsername = username,
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
