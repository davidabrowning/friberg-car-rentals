using FribergCarRentals.Mvc.Areas.Public.Views.Customer;
using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Mvc.Session;

namespace FribergCarRentals.Mvc.Areas.Public.Controllers
{
    [Area("Public")]
    public class CustomerController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly ICRUDApiClient<CustomerDto> _customerDtoApiClient;
        private readonly UserSession _userSession;

        public CustomerController(IUserApiClient userApiClient, ICRUDApiClient<CustomerDto> customerDtoApiClient, UserSession userSession)
        {
            _userApiClient = userApiClient;
            _customerDtoApiClient = customerDtoApiClient;
            _userSession = userSession;
        }

        // GET: Public/Customer
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        // GET: Public/Customer/Create
        public async Task<IActionResult> Create()
        {
            if (!_userSession.IsSignedIn())
            {
                return RedirectToAction("Signin", "Session");
            }

            if (_userSession.IsCustomer())
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

            if (!_userSession.IsSignedIn())
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

            CustomerDto newCustomerDto = new()
            {
                UserId = _userSession.UserDto.UserId,
                FirstName = populatedCustomerCreateVM.FirstName,
                LastName = populatedCustomerCreateVM.LastName,
                HomeCity = populatedCustomerCreateVM.HomeCity,
                HomeCountry = populatedCustomerCreateVM.HomeCountry,
            };

            await _customerDtoApiClient.PostAsync(newCustomerDto);
            await _userSession.UpdateDto();

            TempData["SuccessMessage"] = UserMessage.SuccessCustomerCreated;
            return RedirectToAction("Index");
        }
    }
}
