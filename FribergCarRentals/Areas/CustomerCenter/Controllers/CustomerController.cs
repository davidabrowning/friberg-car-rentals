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
        private readonly IUserApiClient _userApiClient;
        private readonly ICRUDApiClient<CustomerDto> _customerDtoApiClient;
        public CustomerController(IUserApiClient userApiClient, ICRUDApiClient<CustomerDto> customerDtoApiClient)
        {
            _userApiClient = userApiClient;
            _customerDtoApiClient = customerDtoApiClient;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        // GET: CustomerCenter/Customer/Details
        public async Task<IActionResult> Details()
        {
            UserDto userDto = await _userApiClient.GetCurrentUserAsync();
            if (userDto.CustomerDto == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
            }

            DetailsCustomerViewModel detailsCustomerViewModel = new()
            {
                Id = userDto.CustomerDto.Id,
                FirstName = userDto.CustomerDto.FirstName,
                LastName = userDto.CustomerDto.LastName,
                HomeCity = userDto.CustomerDto.HomeCity,
                HomeCountry = userDto.CustomerDto.HomeCountry,
            };

            return View(detailsCustomerViewModel);
        }

        // GET: CustomerCenter/Customer/Edit
        public async Task<IActionResult> Edit()
        {
            UserDto userDto = await _userApiClient.GetCurrentUserAsync();
            if (userDto.CustomerDto == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
            }

            EditCustomerViewModel editCustomerViewModel = new()
            {
                Id = userDto.CustomerDto.Id,
                FirstName = userDto.CustomerDto.FirstName,
                LastName = userDto.CustomerDto.LastName,
                HomeCity = userDto.CustomerDto.HomeCity,
                HomeCountry = userDto.CustomerDto.HomeCountry,
            };

            return View(editCustomerViewModel);
        }

        // POST: CustomerCenter/Customer/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCustomerViewModel editCustomerViewModel)
        {
            UserDto userDto = await _userApiClient.GetCurrentUserAsync();
            if (userDto.CustomerDto == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
            }

            if (userDto.CustomerDto.Id != editCustomerViewModel.Id)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsInvalid;
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(editCustomerViewModel);
            }

            userDto.CustomerDto.FirstName = editCustomerViewModel.FirstName;
            userDto.CustomerDto.LastName = editCustomerViewModel.LastName;
            userDto.CustomerDto.HomeCity = editCustomerViewModel.HomeCity;
            userDto.CustomerDto.HomeCountry = editCustomerViewModel.HomeCountry;
            await _customerDtoApiClient.PutAsync(userDto.CustomerDto);

            TempData["SuccessMessage"] = UserMessage.SuccessCustomerUpdated;
            return RedirectToAction("Details");
        }
    }
}
