using FribergCarRentals.Core.Constants;
using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.Mvc.Areas.Administration.Views.ApplicationUser;
using FribergCarRentals.Mvc.Attributes;
using FribergCarRentals.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Mvc.Areas.Administration.Controllers
{
    [RequireAdmin]
    [Area("Administration")]
    public class ApplicationUserController : Controller
    {
        private readonly ICRUDApiClient<AdminDto> _adminDtoApiClient;
        private readonly ICRUDApiClient<CustomerDto> _customerDtoApiClient;
        private readonly IUserApiClient _userApiClient;

        public ApplicationUserController(ICRUDApiClient<AdminDto> adminDtoApiClient, ICRUDApiClient<CustomerDto> customerDtoApiClient, IUserApiClient userApiClient)
        {
            _adminDtoApiClient = adminDtoApiClient;
            _customerDtoApiClient = customerDtoApiClient;
            _userApiClient = userApiClient;
        }

        // GET: ApplicationUserIndexViewModels
        public async Task<IActionResult> Index()
        {
            List<IndexApplicationUserViewModel> indexApplicationUserViewModelList = new();
            IEnumerable<UserDto> userDtos = await _userApiClient.GetAsync();
            foreach (UserDto userDto in userDtos)
            {
                indexApplicationUserViewModelList.Add(await CreateIndexApplicationUserViewModel(userDto.UserId));
            }
            return View(indexApplicationUserViewModelList);
        }

        // GET: ApplicationUserViewModels/Create
        public IActionResult Create()
        {
            CreateApplicationUserViewModel createApplicationUserViewModel = new();
            return View(createApplicationUserViewModel);
        }

        // POST: ApplicationUserViewModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateApplicationUserViewModel createApplicationUserViewModel)
        {
            UserDto userDto = await _userApiClient.GetByUsernameAsync(createApplicationUserViewModel.Username);
            if (userDto.UserId != null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUsernameAlreadyTaken;
                return View(createApplicationUserViewModel);
            }

            if (!ModelState.IsValid)
            {
                return View(createApplicationUserViewModel);
            }

            await _userApiClient.CreateUserFromUsernameAsync(createApplicationUserViewModel.Username);

            TempData["SuccessMessage"] = UserMessage.SuccessUserCreated;
            return RedirectToAction("Index");
        }

        // GET: ApplicationUser/Edit/5
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

            EditApplicationUserViewModel editApplicationUserViewModel = new EditApplicationUserViewModel()
            {
                UserId = userDto.UserId,
                Username = userDto.Username,
            };
            return View(editApplicationUserViewModel);
        }

        // POST: ApplicationUser/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string userId, EditApplicationUserViewModel editApplicationUserViewModel)
        {
            if (userId != editApplicationUserViewModel.UserId)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsInvalid;
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(editApplicationUserViewModel);
            }

            await _userApiClient.UpdateUsernameAsync(userId, editApplicationUserViewModel.Username);

            TempData["SuccessMessage"] = UserMessage.SuccessUserUpdated;
            return RedirectToAction("Index");
        }

        // GET: ApplicationUser/Delete/5
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

            DeleteApplicationUserViewModel deleteApplicationUserViewModel = new DeleteApplicationUserViewModel()
            {
                UserId = userDto.UserId,
                Username = userDto.Username,
            };
            return View(deleteApplicationUserViewModel);
        }

        // POST: ApplicationUser/DeleteAsync/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string userId)
        {
            await _userApiClient.DeleteUserAsync(userId);

            TempData["SuccessMessage"] = UserMessage.SuccessUserDeleted;
            return RedirectToAction("Index");
        }

        private async Task<IndexApplicationUserViewModel> CreateIndexApplicationUserViewModel(string userId)
        {
            UserDto userDto = await _userApiClient.GetAsync(userId);
            IndexApplicationUserViewModel indexApplicationUserViewModel = new()
            {
                UserId = userDto.UserId,
                Username = userDto.Username,
                IsAdmin = userDto.AuthRoles.Where(r => r == AuthRoleName.Admin).Any(),
                IsCustomer = userDto.AuthRoles.Where(r => r == AuthRoleName.Customer).Any(),
            };
            if (indexApplicationUserViewModel.IsAdmin)
            {
                indexApplicationUserViewModel.AdminId = userDto.AdminDto.Id;
                indexApplicationUserViewModel.AdminName = userDto.AdminDto.ToString();
            }
            if (indexApplicationUserViewModel.IsCustomer)
            {
                indexApplicationUserViewModel.CustomerId = userDto.CustomerDto.Id;
                indexApplicationUserViewModel.CustomerName = $"{userDto.CustomerDto.FirstName} {userDto.CustomerDto.LastName}";
            }
            return indexApplicationUserViewModel;
        }
    }
}
