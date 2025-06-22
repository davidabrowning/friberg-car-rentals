using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using FribergCarRentals.Interfaces;
using FribergCarRentals.Areas.Administration.Views.Admin;

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
                return View(createAdminViewModel);
            }

            IdentityUser? identityUser = await _userService.GetUserById(createAdminViewModel.IdentityUserId);
            if (identityUser == null)
            {
                return NotFound();
            }

            Admin admin = new()
            {
                IdentityUser = identityUser
            };
            await _userService.CreateAdminAsync(admin);

            return RedirectToAction("Index");
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
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(editAdminViewModel);
            }

            Admin admin = await _userService.GetAdminByIdAsync((int)id);
            if (admin == null)
            {
                return NotFound();
            }

            await _userService.UpdateAdminAsync(admin);

            return RedirectToAction("Index");
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
            return RedirectToAction("Index");
        }
    }
}
