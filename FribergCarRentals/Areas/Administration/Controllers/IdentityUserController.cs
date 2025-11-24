using FribergCarRentals.Mvc.Areas.Administration.Views.IdentityUser;
using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Core.Constants;

namespace FribergCarRentals.Mvc.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administration")]
    public class IdentityUserController : Controller
    {
        private readonly ICRUDApiClient<AdminDto> _adminDtoApiClient;
        private readonly ICRUDApiClient<CustomerDto> _customerDtoApiClient;
        private readonly IUserApiClient _userApiClient;

        public IdentityUserController(ICRUDApiClient<AdminDto> adminDtoApiClient, ICRUDApiClient<CustomerDto> customerDtoApiClient, IUserApiClient userApiClient)
        {
            _adminDtoApiClient = adminDtoApiClient;
            _customerDtoApiClient = customerDtoApiClient;
            _userApiClient = userApiClient;
        }

        // GET: IdentityUserIndexViewModels
        public async Task<IActionResult> Index()
        {
            List<IndexIdentityUserViewModel> indexIdentityUserViewModelList = new();
            IEnumerable<UserDto> userDtos = await _userApiClient.GetAsync();
            foreach (UserDto userDto in userDtos)
            {
                indexIdentityUserViewModelList.Add(await CreateIndexIdentityUserViewModel(userDto.UserId));
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
            UserDto userDto = await _userApiClient.GetByUsernameAsync(createIdentityUserViewModel.Username);
            if (userDto.UserId != null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUsernameAlreadyTaken;
                return View(createIdentityUserViewModel);
            }

            if (!ModelState.IsValid)
            {
                return View(createIdentityUserViewModel);
            }

            await _userApiClient.CreateUserFromUsernameAsync(createIdentityUserViewModel.Username);

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

            UserDto userDto = await _userApiClient.GetAsync(userId);
            if (userDto.UserId == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

            EditIdentityUserViewModel editIdentityUserViewModel = new EditIdentityUserViewModel()
            {
                IdentityUserId = userDto.UserId,
                IdentityUserUsername = userDto.Username,
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

            await _userApiClient.UpdateUsernameAsync(userId, editIdentityUserViewModel.IdentityUserUsername);

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

            UserDto userDto = await _userApiClient.GetAsync(userId);
            if (userDto.UserId == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

            DeleteIdentityUserViewModel deleteIdentityUserViewModel = new DeleteIdentityUserViewModel()
            {
                IdentityUserId = userDto.UserId,
                IdentityUserUsername = userDto.Username,
            };
            return View(deleteIdentityUserViewModel);
        }

        // POST: Admin/DeleteAsync/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string userId)
        {
            await _userApiClient.DeleteUserAsync(userId);

            TempData["SuccessMessage"] = UserMessage.SuccessUserDeleted;
            return RedirectToAction("Index");
        }

        private async Task<IndexIdentityUserViewModel> CreateIndexIdentityUserViewModel(string userId)
        {
            UserDto userDto = await _userApiClient.GetAsync(userId);
            IndexIdentityUserViewModel indexIdentityUserViewModel = new()
            {
                IdentityUserId = userDto.UserId,
                IdentityUserUsername = userDto.Username,
                IsAdmin = userDto.AuthRoles.Where(r => r == AuthRoleName.Admin).Any(),
                IsCustomer = userDto.AuthRoles.Where(r => r == AuthRoleName.Customer).Any(),
            };
            if (indexIdentityUserViewModel.IsAdmin)
            {
                indexIdentityUserViewModel.AdminId = userDto.AdminDto.Id;
                indexIdentityUserViewModel.AdminName = userDto.AdminDto.ToString();
            }
            if (indexIdentityUserViewModel.IsCustomer)
            {
                indexIdentityUserViewModel.CustomerId = userDto.CustomerDto.Id;
                indexIdentityUserViewModel.CustomerName = $"{userDto.CustomerDto.FirstName} {userDto.CustomerDto.LastName}";
            }
            return indexIdentityUserViewModel;
        }
    }
}
