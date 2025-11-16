using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Core.Models;
using Microsoft.AspNetCore.Authorization;
using FribergCarRentals.Core.Interfaces;
using FribergCarRentals.Areas.Administration.Views.IdentityUser;
using FribergCarRentals.Core.Helpers;

namespace FribergCarRentals.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administration")]
    public class IdentityUserController : Controller
    {
        private readonly IUserService _userService;

        public IdentityUserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: IdentityUserIndexViewModels
        public async Task<IActionResult> Index()
        {
            List<IndexIdentityUserViewModel> indexIdentityUserViewModelList = new();
            IEnumerable<string> userIds = await _userService.GetAllUserIdsAsync();
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
            if (await _userService.UsernameExistsAsync(createIdentityUserViewModel.Username))
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUsernameAlreadyTaken;
                return View(createIdentityUserViewModel);
            }

            if (!ModelState.IsValid)
            {
                return View(createIdentityUserViewModel);
            }

            await _userService.CreateUser(createIdentityUserViewModel.Username);

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

            string? username = await _userService.GetUsernameByUserId(userId);
            if (username is null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

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
        public async Task<IActionResult> Edit(string id, EditIdentityUserViewModel editIdentityUserViewModel)
        {
            if (id != editIdentityUserViewModel.IdentityUserId)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsInvalid;
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(editIdentityUserViewModel);
            }

            await _userService.UpdateUsername(id, editIdentityUserViewModel.IdentityUserUsername);

            TempData["SuccessMessage"] = UserMessage.SuccessUserUpdated;
            return RedirectToAction("Index");
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsNull;
                return RedirectToAction("Index");
            }

            string? username = await _userService.GetUsernameByUserId(id);
            if (username == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

            DeleteIdentityUserViewModel deleteIdentityUserViewModel = new DeleteIdentityUserViewModel()
            {
                IdentityUserId = id,
                IdentityUserUsername = username,
            };
            return View(deleteIdentityUserViewModel);
        }

        // POST: Admin/DeleteAsync/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _userService.DeleteUserAsync(id);

            TempData["SuccessMessage"] = UserMessage.SuccessUserDeleted;
            return RedirectToAction("Index");
        }

        private async Task<IndexIdentityUserViewModel> CreateIndexIdentityUserViewModel(string userId)
        {
            string username = await _userService.GetUsernameByUserId(userId);
            IndexIdentityUserViewModel indexIdentityUserViewModel = new()
            {
                IdentityUserId = userId,
                IdentityUserUsername = username,
                IsAdmin = await _userService.IsInRoleAsync(userId, "Admin"),
                IsCustomer = await _userService.IsInRoleAsync(userId, "Customer"),
            };
            if (indexIdentityUserViewModel.IsAdmin)
            {
                Admin? admin = await _userService.GetAdminByUserIdAsync(userId);
                if (admin != null)
                {
                    indexIdentityUserViewModel.AdminId = admin.Id;
                    indexIdentityUserViewModel.AdminName = admin.Id.ToString();
                }
            }
            if (indexIdentityUserViewModel.IsCustomer)
            {
                Customer? customer = await _userService.GetCustomerByUserIdAsync(userId);
                if (customer != null)
                {
                    indexIdentityUserViewModel.CustomerId = customer.Id;
                    indexIdentityUserViewModel.CustomerName = $"{customer.FirstName} {customer.LastName}";
                }
            }
            return indexIdentityUserViewModel;
        }
    }
}
