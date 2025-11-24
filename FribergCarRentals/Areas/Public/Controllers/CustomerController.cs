using FribergCarRentals.Mvc.Areas.Public.Views.Customer;
using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Mvc.Areas.Public.Controllers
{
    [Authorize]
    [Area("Public")]
    public class CustomerController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly ICRUDApiClient<CustomerDto> _customerDtoApiClient;

        public CustomerController(IUserApiClient userApiClient, ICRUDApiClient<CustomerDto> customerDtoApiClient)
        {
            _userApiClient = userApiClient;
            _customerDtoApiClient = customerDtoApiClient;
        }

        // GET: Public/Customer
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        // GET: Public/Customer/Create
        public async Task<IActionResult> Create()
        {
            UserDto userDto = await _userApiClient.GetCurrentUserAsync();
            if (userDto.UserId == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index", "Home");
            }

            // Check if already a customer
            if (userDto.CustomerDto != null)
            {
                return RedirectToAction("Index", "Car", new { area = "CustomerCenter" });
            }

            CreateCustomerViewModel emptyVM = new();
            return View(emptyVM);
        }

        // POST: Public/Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerViewModel populatedCustomerCreateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(populatedCustomerCreateVM);
            }

            UserDto userDto = await _userApiClient.GetCurrentUserAsync();
            if (userDto.UserId == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

            CustomerDto newCustomerDto = new()
            {
                UserId = userDto.UserId,
                FirstName = populatedCustomerCreateVM.FirstName,
                LastName = populatedCustomerCreateVM.LastName,
                HomeCity = populatedCustomerCreateVM.HomeCity,
                HomeCountry = populatedCustomerCreateVM.HomeCountry,
            };

            await _customerDtoApiClient.PostAsync(newCustomerDto);

            TempData["SuccessMessage"] = UserMessage.SuccessCustomerCreated;
            return RedirectToAction("Index");
        }
    }
}
