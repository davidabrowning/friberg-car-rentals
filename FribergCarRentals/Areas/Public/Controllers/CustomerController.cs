using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.Mvc.Areas.Public.Views.Customer;
using FribergCarRentals.Mvc.Session;
using FribergCarRentals.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Mvc.Areas.Public.Controllers
{
    [Area("Public")]
    public class CustomerController : Controller
    {
        private readonly ICRUDApiClient<CustomerDto> _customerDtoApiClient;
        private readonly UserSession _userSession;

        public CustomerController(ICRUDApiClient<CustomerDto> customerDtoApiClient, UserSession userSession)
        {
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

            try
            {
                CustomerDto newCustomerDto = new()
                {
                    UserId = (await _userSession.GetUserDto()).UserId,
                    FirstName = populatedCustomerCreateVM.FirstName,
                    LastName = populatedCustomerCreateVM.LastName,
                    HomeCity = populatedCustomerCreateVM.HomeCity,
                    HomeCountry = populatedCustomerCreateVM.HomeCountry,
                };

                await _customerDtoApiClient.PostAsync(newCustomerDto);
                _userSession.HasBecomeCustomerMidSession = true;

                TempData["SuccessMessage"] = UserMessage.SuccessCustomerCreated;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
