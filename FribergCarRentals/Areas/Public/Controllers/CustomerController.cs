using FribergCarRentals.Areas.Public.Views.Customer;
using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Areas.Public.Controllers
{
    [Authorize]
    [Area("Public")]
    public class CustomerController : Controller
    {
        private readonly IAuthApiClient _authApiClient;
        private readonly ICRUDApiClient<CustomerDto> _customerDtoApiClient;

        public CustomerController(IAuthApiClient authApiClient, ICRUDApiClient<CustomerDto> customerDtoApiClient)
        {
            _authApiClient = authApiClient;
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
            string? signedInUserId = await _authApiClient.GetCurrentSignedInUserIdAsync();
            if (signedInUserId == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index", "Home");
            }
            int signedInCustomerId = await _authApiClient.GetCustomerIdByUserId(signedInUserId);
            CustomerDto? signedInCustomerDto = await _customerDtoApiClient.GetAsync(signedInCustomerId);
            if (signedInCustomerDto != null)
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

            string? userId = await _authApiClient.GetCurrentSignedInUserIdAsync();
            if (userId == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

            CustomerDto newCustomerDto = new()
            {
                UserId = userId,
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
