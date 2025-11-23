using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Core.Models;
using Microsoft.AspNetCore.Authorization;
using FribergCarRentals.Areas.Administration.Views.Customer;
using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;
using FribergCarRentals.WebApi.Mappers;

namespace FribergCarRentals.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administration")]
    public class CustomerController : Controller
    {
        private readonly ICRUDApiClient<CustomerDto> _customerDtoApiClient;
        private readonly IAuthApiClient _authApiClient;

        public CustomerController( ICRUDApiClient<CustomerDto> customerDtoApiClient, IAuthApiClient authApiClient)
        {
            _customerDtoApiClient = customerDtoApiClient;
            _authApiClient = authApiClient;
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

            bool isUser = await _authApiClient.IsUserAsync(userId);
            if (!isUser)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

            string username = await _authApiClient.GetUsernameByUserIdAsync(userId);
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
            bool isUser = await _authApiClient.IsUserAsync(userId);
            if (!isUser)
            {
                return NotFound();
            }

            CustomerDto customerDto = new()
            {
                UserId = userId,
                FirstName = createCustomerViewModel.FirstName,
                LastName = createCustomerViewModel.LastName,
                HomeCity = createCustomerViewModel.HomeCity,
                HomeCountry = createCustomerViewModel.HomeCountry,
            };
            await _customerDtoApiClient.PostAsync(customerDto);

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

            string username = await _authApiClient.GetUsernameByUserIdAsync(customerDto.UserId);
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

            CustomerDto? customerDto = await _customerDtoApiClient.GetAsync(editCustomerViewModel.CustomerId);

            if (customerDto == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
            }

            customerDto.FirstName = editCustomerViewModel.FirstName;
            customerDto.LastName = editCustomerViewModel.LastName;
            customerDto.HomeCity = editCustomerViewModel.HomeCity;
            customerDto.HomeCountry = editCustomerViewModel.HomeCountry;
            await _customerDtoApiClient.PutAsync(customerDto);

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

            CustomerDto? customerDto = await _customerDtoApiClient.GetAsync((int)id);
            if (customerDto == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
            }

            string username = await _authApiClient.GetUsernameByUserIdAsync(customerDto.UserId);
            DeleteCustomerViewModel deleteCustomerViewModel = new DeleteCustomerViewModel()
            {
                CustomerId = customerDto.Id,
                IdentityUserId = customerDto.UserId,
                IdentityUserUsername = username,
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                HomeCity = customerDto.HomeCity,
                HomeCountry = customerDto.HomeCountry,
            };

            return View(deleteCustomerViewModel);
        }

        // POST: Customer/DeleteAsync/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _customerDtoApiClient.DeleteAsync(id);

            TempData["SuccessMessage"] = UserMessage.SuccessCustomerDeleted;
            return RedirectToAction("Index");
        }
    }
}
