using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.Data;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Areas.Administration.ViewModels;
using Microsoft.AspNetCore.Authorization;
using FribergCarRentals.Areas.Administration.Helpers;

namespace FribergCarRentals.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administration")]
    public class AdminController : Controller
    {
        private readonly IRepository<Admin> _adminRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(IRepository<Admin> adminRepository, UserManager<IdentityUser> userManager)
        {
            _adminRepository = adminRepository;
            _userManager = userManager;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdentityUserId")] AdminCreateViewModel adminCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(adminCreateViewModel);
            }
                
            if (!ModelState.IsValid)
            {
                return View(adminCreateViewModel);
            }

            IdentityUser? identityUser = await _userManager.Users.Where(u => u.Id == adminCreateViewModel.IdentityUserId).FirstOrDefaultAsync();
            if (identityUser == null)
                return NotFound();

            Admin admin = ViewModelMappingHelper.GetAdmin(adminCreateViewModel, identityUser);
            await _adminRepository.Add(admin);
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Admin? admin = await _adminRepository.GetById((int)id);
            if (admin == null)
            {
                return NotFound();
            }

            AdminEditViewModel adminViewModel = ViewModelMappingHelper.GetAdminViewModel(admin);
            return View(adminViewModel);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdminId")] AdminEditViewModel adminEditViewModel)
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
                Admin admin = await _adminRepository.GetById((int)id);
                await _adminRepository.Update(admin);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AdminExists(adminEditViewModel.AdminId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Admin? admin = await _adminRepository.GetById((int)id);
            if (admin == null)
            {
                return NotFound();
            }

            AdminEditViewModel adminViewModel = ViewModelMappingHelper.GetAdminViewModel(admin);
            return View(adminViewModel);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _adminRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> AdminExists(int id)
        {
            return await _adminRepository.IdExists(id);
        }
    }
}
