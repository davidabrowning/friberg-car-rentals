using FribergCarRentals.Mvc.Areas.CustomerCenter.Views.Customer;
using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Mvc.Areas.CustomerCenter.Controllers
{
    [Authorize(Roles = "Customer")]
    [Area("CustomerCenter")]
    public class CustomerController : Controller
    {
        private readonly IAuthApiClient _authApiClient;
        private readonly ICRUDApiClient<CustomerDto> _customerDtoApiClient;
        public CustomerController(IAuthApiClient authApiClient, ICRUDApiClient<CustomerDto> customerDtoApiClient)
        {
            _authApiClient = authApiClient;
            _customerDtoApiClient = customerDtoApiClient;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        // GET: CustomerCenter/Customer/Details
        public async Task<IActionResult> Details()
        {
            string? userId = await _authApiClient.GetCurrentSignedInUserIdAsync();
            if (userId == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

            int customerId = await _authApiClient.GetCustomerIdByUserId(userId);
            CustomerDto? customerDto = await _customerDtoApiClient.GetAsync(customerId);
            if (customerDto == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
            }

            DetailsCustomerViewModel detailsCustomerViewModel = new()
            {
                Id = customerDto.Id,
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                HomeCity = customerDto.HomeCity,
                HomeCountry = customerDto.HomeCountry,
            };

            return View(detailsCustomerViewModel);
        }

        // GET: CustomerCenter/Customer/Edit
        public async Task<IActionResult> Edit()
        {
            string? userId = await _authApiClient.GetCurrentSignedInUserIdAsync();
            if (userId == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

            int customerId = await _authApiClient.GetCustomerIdByUserId(userId);
            CustomerDto? customerDto = await _customerDtoApiClient.GetAsync(customerId);
            if (customerDto == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
            }

            EditCustomerViewModel editCustomerViewModel = new()
            {
                Id = customerDto.Id,
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                HomeCity = customerDto.HomeCity,
                HomeCountry = customerDto.HomeCountry,
            };

            return View(editCustomerViewModel);
        }

        // POST: CustomerCenter/Customer/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCustomerViewModel editCustomerViewModel)
        {
            string? userId = await _authApiClient.GetCurrentSignedInUserIdAsync();
            if (userId == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

            int customerId = await _authApiClient.GetCustomerIdByUserId(userId);
            CustomerDto? customerDto = await _customerDtoApiClient.GetAsync(customerId);
            if (customerDto == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
            }

            if (customerDto.Id != editCustomerViewModel.Id)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsInvalid;
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(editCustomerViewModel);
            }

            customerDto.FirstName = editCustomerViewModel.FirstName;
            customerDto.LastName = editCustomerViewModel.LastName;
            customerDto.HomeCity = editCustomerViewModel.HomeCity;
            customerDto.HomeCountry = editCustomerViewModel.HomeCountry;
            await _customerDtoApiClient.PutAsync(customerDto);

            TempData["SuccessMessage"] = UserMessage.SuccessCustomerUpdated;
            return RedirectToAction("Details");
        }
    }
}
