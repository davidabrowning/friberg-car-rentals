using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Authorization;
using FribergCarRentals.Interfaces;
using FribergCarRentals.Areas.Administration.Views.IdentityUser;

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
            IEnumerable<IdentityUser> identityUsers = await _userService.GetAllIdentityUsersAsync();
            foreach (IdentityUser identityUser in identityUsers)
            {
                indexIdentityUserViewModelList.Add(await CreateIndexIdentityUserViewModel(identityUser));
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
            if (await _userService.IdentityUsernameExistsAsync(createIdentityUserViewModel.Username))
            {
                return View(createIdentityUserViewModel);
            }

            if (!ModelState.IsValid)
            {
                return View(createIdentityUserViewModel);
            }

            await _userService.CreateUser(createIdentityUserViewModel.Username);

            return RedirectToAction("Index");
        }

        // GET: IdentityUserViewModels/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IdentityUser? identityUser = await _userService.GetUserById(id);
            if (identityUser is null)
            {
                return NotFound();
            }

            EditIdentityUserViewModel editIdentityUserViewModel = new EditIdentityUserViewModel()
            {
                IdentityUserId = identityUser.Id,
                IdentityUserUsername = identityUser.UserName,
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
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(editIdentityUserViewModel);
            }

            await _userService.UpdateUsername(id, editIdentityUserViewModel.IdentityUserUsername);

            return RedirectToAction("Index");
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IdentityUser? identityUser = await _userService.GetUserById(id);
            if (identityUser == null)
            {
                return NotFound();
            }

            DeleteIdentityUserViewModel deleteIdentityUserViewModel = new DeleteIdentityUserViewModel()
            {
                IdentityUserId = identityUser.Id,
                IdentityUserUsername = identityUser.UserName ?? "Unable to fetch username",
            };
            return View(deleteIdentityUserViewModel);
        }

        // POST: Admin/DeleteAsync/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _userService.DeleteIdentityUserAsync(id);

            return RedirectToAction("Index");
        }

        private async Task<IndexIdentityUserViewModel> CreateIndexIdentityUserViewModel(IdentityUser user)
        {
            IndexIdentityUserViewModel indexIdentityUserViewModel = new()
            {
                IdentityUserId = user.Id,
                IdentityUserUsername = user.UserName,
                IsAdmin = await _userService.IsInRoleAsync(user, "Admin"),
                IsCustomer = await _userService.IsInRoleAsync(user, "Customer"),
            };
            if (indexIdentityUserViewModel.IsAdmin)
            {
                Admin? admin = await _userService.GetAdminByUserAsync(user);
                if (admin != null)
                {
                    indexIdentityUserViewModel.AdminId = admin.Id;
                    indexIdentityUserViewModel.AdminName = admin.Id.ToString();
                }
            }
            if (indexIdentityUserViewModel.IsCustomer)
            {
                Customer? customer = await _userService.GetCustomerByUserAsync(user);
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
