using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using FribergCarRentals.Interfaces;
using FribergCarRentals.Areas.Administration.Views.Admin;
using FribergCarRentals.Helpers;

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
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

            CreateAdminViewModel createAdminViewModel = new()
            {
                IdentityUserId = identityUserId,
            };
            return View(createAdminViewModel);
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAdminViewModel createAdminViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["WarningMessage"] = UserMessage.WarningInvalidFormData;
                return View(createAdminViewModel);
            }

            string userId = createAdminViewModel.IdentityUserId;
            string? username = await _userService.GetUsernameByUserId(userId);
            if (username == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

            Admin admin = new()
            {
                UserId = userId
            };
            await _userService.CreateAdminAsync(admin);

            TempData["SuccessMessage"] = UserMessage.SuccessAdminCreated;
            return RedirectToAction("Index");
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsNull;
                return RedirectToAction("Index");
            }

            Admin? admin = await _userService.GetAdminByAdminIdAsync((int)id);
            if (admin == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorAdminIsNull;
                return RedirectToAction("Index");
            }

            EditAdminViewModel editAdminViewModel = new()
            {
                AdminId = admin.Id
            };
            return View(editAdminViewModel);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditAdminViewModel editAdminViewModel)
        {
            if (id != editAdminViewModel.AdminId)
            {

                TempData["ErrorMessage"] = UserMessage.ErrorAdminIsNull;
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                TempData["WarningMessage"] = UserMessage.WarningInvalidFormData;
                return View(editAdminViewModel);
            }

            Admin? admin = await _userService.GetAdminByAdminIdAsync((int)id);
            if (admin == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorAdminIsNull;
                return RedirectToAction("Index");
            }

            await _userService.UpdateAdminAsync(admin);

            TempData["SuccessMessage"] = UserMessage.SuccessAdminUpdated;
            return RedirectToAction("Index");
        }

        // GET: Admin/DeleteAsync/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsNull;
                return RedirectToAction("Index");
            }

            Admin? admin = await _userService.GetAdminByAdminIdAsync((int)id);
            if (admin == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorAdminIsNull;
                return RedirectToAction("Index");
            }

            DeleteAdminViewModel deleteAdminViewModel = new DeleteAdminViewModel()
            {
                AdminId = admin.Id,
            };

            return View(deleteAdminViewModel);
        }

        // POST: Admin/DeleteAsync/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _userService.DeleteAdminAsync(id);

            TempData["SuccessMessage"] = UserMessage.SuccessAdminDeleted;
            return RedirectToAction("Index");
        }
    }
}
