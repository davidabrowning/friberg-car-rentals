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
    public class IdentityUserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IdentityUserController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: IdentityUserViewModels
        public async Task<IActionResult> Index()
        {
            List<IdentityUserViewModel> identityUserViewModels = new();
            List<IdentityUser> users = _userManager.Users.ToList();
            foreach (IdentityUser user in users)
            {
                IdentityUserViewModel identityUserViewModel = new()
                {
                    Id = user.Id,
                    Username = user.UserName,
                    IsAdmin = await _userManager.IsInRoleAsync(user, "Admin"),
                    IsUser = await _userManager.IsInRoleAsync(user, "User"),
                };
                identityUserViewModels.Add(identityUserViewModel);
            }
            return View(identityUserViewModels);
        }

        // GET: IdentityUserViewModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IdentityUserViewModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,IsAdmin,IsUser")] IdentityUserViewModel identityUserViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(identityUserViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(identityUserViewModel);
        }

        // GET: IdentityUserViewModels/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IdentityUser user = await _userManager.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }

            IdentityUserViewModel identityUserViewModel = new();
            await ViewModelMappingHelper.MapAToB(user, identityUserViewModel, _userManager);
            return View(identityUserViewModel);
        }

        // POST: IdentityUserViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Username,IsAdmin,IsUser")] IdentityUserViewModel identityUserViewModel)
        {
            if (id != identityUserViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    IdentityUser user = await _userManager.Users.Where(u => u.Id == identityUserViewModel.Id).FirstOrDefaultAsync();
                    await ViewModelMappingHelper.MapAToB(identityUserViewModel, user, _userManager);
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(identityUserViewModel);
        }
    }
}
