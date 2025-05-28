using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.Data;
using FribergCarRentals.Models;
using FribergCarRentals.Helpers;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Areas.Administration.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace FribergCarRentals.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administration")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            List<AdminViewModel> adminViewModels = new();
            List<Admin> admins = await _context.Admins.ToListAsync();
            foreach (Admin admin in admins)
            {
                AdminViewModel adminViewModel = new();
                ViewModelMappingHelper.MapAToB(admin, adminViewModel);
                adminViewModels.Add(adminViewModel);
            }
            return View(adminViewModels);
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Admin admin = await _context.Admins.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (admin == null)
            {
                return NotFound();
            }

            AdminViewModel adminViewModel = new();
            ViewModelMappingHelper.MapAToB(admin, adminViewModel);
            return View(adminViewModel);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName")] AdminViewModel adminViewModel)
        {
            if (ModelState.IsValid)
            {
                Admin admin = new();
                ViewModelMappingHelper.MapAToB(adminViewModel, admin);
                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adminViewModel);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Admin admin = await _context.Admins.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (admin == null)
            {
                return NotFound();
            }

            AdminViewModel adminViewModel = new();
            ViewModelMappingHelper.MapAToB(admin, adminViewModel);
            return View(adminViewModel);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName")] AdminViewModel adminViewModel)
        {
            if (id != adminViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Admin admin = await _context.Admins.Where(a => a.Id == id).FirstOrDefaultAsync();
                    ViewModelMappingHelper.MapAToB(adminViewModel, admin);
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(adminViewModel.Id))
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
            return View(adminViewModel);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Admin admin = await _context.Admins.FirstOrDefaultAsync(m => m.Id == id);
            if (admin == null)
            {
                return NotFound();
            }

            AdminViewModel adminViewModel = new();
            ViewModelMappingHelper.MapAToB(admin, adminViewModel);
            return View(adminViewModel);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Admin admin = await _context.Admins.FindAsync(id);
            if (admin != null)
            {
                _context.Admins.Remove(admin);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminExists(int id)
        {
            return _context.Admins.Any(a => a.Id == id);
        }
    }
}
