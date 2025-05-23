using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.Data;
using FribergCarRentals.ViewModels;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Models;
using FribergCarRentals.Helpers;

namespace FribergCarRentals.Controllers
{
    public class ApplicationUserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ApplicationUserViewModels
        public async Task<IActionResult> Index()
        {
            List<ApplicationUserViewModel> applicationUserViewModels = new();
            List<ApplicationUser> applicationUsers = _userManager.Users.ToList();
            foreach (ApplicationUser applicationUser in applicationUsers)
            {
                ApplicationUserViewModel applicationUserViewModel = new()
                {
                    Id = applicationUser.Id,
                    Username = applicationUser.UserName,
                    IsAdmin = await _userManager.IsInRoleAsync(applicationUser, "Admin"),
                    IsUser = await _userManager.IsInRoleAsync(applicationUser, "User"),
                };
                applicationUserViewModels.Add(applicationUserViewModel);
            }
            return View(applicationUserViewModels);
        }

        // GET: ApplicationUserViewModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationUserViewModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,IsAdmin,IsUser")] ApplicationUserViewModel applicationUserViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationUserViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUserViewModel);
        }

        // GET: ApplicationUserViewModels/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser applicationUser = await _userManager.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (applicationUser == null)
            {
                return NotFound();
            }

            ApplicationUserViewModel applicationUserViewModel = new();
            await ViewModelMappingHelper.MapAToB(applicationUser, applicationUserViewModel, _userManager);
            return View(applicationUserViewModel);
        }

        // POST: ApplicationUserViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Username,IsAdmin,IsUser")] ApplicationUserViewModel applicationUserViewModel)
        {
            if (id != applicationUserViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser applicationUser = await _userManager.Users.Where(u => u.Id == applicationUserViewModel.Id).FirstOrDefaultAsync();
                    await ViewModelMappingHelper.MapAToB(applicationUserViewModel, applicationUser, _userManager);
                    _context.Update(applicationUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUserViewModel);
        }
    }
}
