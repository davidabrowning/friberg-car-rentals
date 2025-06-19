using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Areas.Administration.ViewModels;
using Microsoft.AspNetCore.Authorization;
using FribergCarRentals.Interfaces;

namespace FribergCarRentals.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administration")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: Admin
        public IActionResult Index()
        {
            return RedirectToAction("Index", "IdentityUser");
        }

        // GET: Admin/Create/abcd-1234
        [HttpGet("Administration/Admin/Create/{identityUserId}")]
        public IActionResult Create(string? identityUserId)
        {
            if (identityUserId == null)
            {
                return NotFound();
            }

            AdminCreateViewModel adminCreateViewModel = new()
            {
                IdentityUserId = identityUserId,
            };
            return View(adminCreateViewModel);
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminCreateViewModel adminCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(adminCreateViewModel);
            }

            IdentityUser? identityUser = await _userService.GetUserById(adminCreateViewModel.IdentityUserId);
            if (identityUser == null)
            {
                return NotFound();
            }

            Admin admin = new()
            {
                IdentityUser = identityUser
            };
            await _userService.CreateAdminAsync(admin);

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Admin? admin = await _userService.GetAdminByIdAsync((int)id);
            if (admin == null)
            {
                return NotFound();
            }

            AdminEditViewModel adminViewModel = new()
            {
                AdminId = admin.Id
            };
            return View(adminViewModel);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminEditViewModel adminEditViewModel)
        {
            if (id != adminEditViewModel.AdminId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(adminEditViewModel);
            }

            try
            {
                Admin admin = await _userService.GetAdminByIdAsync((int)id);
                // ViewModelToUpdateHelper.UpdateExistingAdmin(admin, adminEditViewModel); - not yet implemented
                await _userService.UpdateAdminAsync(admin);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/DeleteAsync/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Admin? admin = await _userService.GetAdminByIdAsync((int)id);
            if (admin == null)
            {
                return NotFound();
            }

            AdminEditViewModel adminEditViewModel = new AdminEditViewModel()
            {
                AdminId = admin.Id,
            };

            return View(adminEditViewModel);
        }

        // POST: Admin/DeleteAsync/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _userService.DeleteAdminAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
