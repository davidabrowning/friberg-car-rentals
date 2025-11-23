using FribergCarRentals.Mvc.Areas.Administration.Views.IdentityUser;
using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Mvc.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administration")]
    public class IdentityUserController : Controller
    {
        private readonly ICRUDApiClient<AdminDto> _adminDtoApiClient;
        private readonly ICRUDApiClient<CustomerDto> _customerDtoApiClient;
        private readonly IAuthApiClient _authApiClient;

        public IdentityUserController(ICRUDApiClient<AdminDto> adminDtoApiClient, ICRUDApiClient<CustomerDto> customerDtoApiClient, IAuthApiClient authApiClient)
        {
            _adminDtoApiClient = adminDtoApiClient;
            _customerDtoApiClient = customerDtoApiClient;
            _authApiClient = authApiClient;
        }

        // GET: IdentityUserIndexViewModels
        public async Task<IActionResult> Index()
        {
            List<IndexIdentityUserViewModel> indexIdentityUserViewModelList = new();
            IEnumerable<string> userIds = await _authApiClient.GetAllUserIdsAsync();
            foreach (string userId in userIds)
            {
                indexIdentityUserViewModelList.Add(await CreateIndexIdentityUserViewModel(userId));
            }
            return View(indexIdentityUserViewModelList);
        }

        // GET: IdentityUserViewModels/Create
        public IActionResult Create()
        {
            CreateIdentityUserViewModel createIdentityUserViewModel = new();
            return View(createIdentityUserViewModel);
        }

        // POST: IdentityUserViewModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateIdentityUserViewModel createIdentityUserViewModel)
        {
            bool isUser = await _authApiClient.IsUserAsync(createIdentityUserViewModel.Username);
            if (isUser)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUsernameAlreadyTaken;
                return View(createIdentityUserViewModel);
            }

            if (!ModelState.IsValid)
            {
                return View(createIdentityUserViewModel);
            }

            await _authApiClient.CreateUserAsync(createIdentityUserViewModel.Username);

            TempData["SuccessMessage"] = UserMessage.SuccessUserCreated;
            return RedirectToAction("Index");
        }

        // GET: IdentityUserViewModels/Edit/5
        public async Task<IActionResult> Edit(string userId)
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
            EditIdentityUserViewModel editIdentityUserViewModel = new EditIdentityUserViewModel()
            {
                IdentityUserId = userId,
                IdentityUserUsername = username,
            };
            return View(editIdentityUserViewModel);
        }

        // POST: IdentityUserViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string userId, EditIdentityUserViewModel editIdentityUserViewModel)
        {
            if (userId != editIdentityUserViewModel.IdentityUserId)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsInvalid;
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(editIdentityUserViewModel);
            }

            await _authApiClient.UpdateUsernameAsync(userId, editIdentityUserViewModel.IdentityUserUsername);

            TempData["SuccessMessage"] = UserMessage.SuccessUserUpdated;
            return RedirectToAction("Index");
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(string? userId)
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
            DeleteIdentityUserViewModel deleteIdentityUserViewModel = new DeleteIdentityUserViewModel()
            {
                IdentityUserId = userId,
                IdentityUserUsername = username,
            };
            return View(deleteIdentityUserViewModel);
        }

        // POST: Admin/DeleteAsync/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string userId)
        {
            await _authApiClient.DeleteUserAsync(userId);

            TempData["SuccessMessage"] = UserMessage.SuccessUserDeleted;
            return RedirectToAction("Index");
        }

        private async Task<IndexIdentityUserViewModel> CreateIndexIdentityUserViewModel(string userId)
        {
            string username = await _authApiClient.GetUsernameByUserIdAsync(userId);
            IndexIdentityUserViewModel indexIdentityUserViewModel = new()
            {
                IdentityUserId = userId,
                IdentityUserUsername = username,
                IsAdmin = await _authApiClient.IsInRoleAsync(userId, "Admin"),
                IsCustomer = await _authApiClient.IsInRoleAsync(userId, "Customer"),
            };
            if (indexIdentityUserViewModel.IsAdmin)
            {
                bool userIsAdmin = await _authApiClient.IsAdminAsync(userId);
                if (userIsAdmin)
                {
                    int adminId = await _authApiClient.GetAdminIdByUserId(userId);
                    AdminDto adminDto = await _adminDtoApiClient.GetAsync(adminId);
                    indexIdentityUserViewModel.AdminId = adminDto.Id;
                    indexIdentityUserViewModel.AdminName = adminDto.Id.ToString();
                }
            }
            if (indexIdentityUserViewModel.IsCustomer)
            {
                bool userIsCustomer = await _authApiClient.IsCustomerAsync(userId);
                if (userIsCustomer)
                {
                    int customerId = await _authApiClient.GetCustomerIdByUserId(userId);
                    CustomerDto? customerDto = await _customerDtoApiClient.GetAsync(customerId);
                    indexIdentityUserViewModel.CustomerId = customerDto.Id;
                    indexIdentityUserViewModel.CustomerName = $"{customerDto.FirstName} {customerDto.LastName}";
                }
            }
            return indexIdentityUserViewModel;
        }
    }
}
