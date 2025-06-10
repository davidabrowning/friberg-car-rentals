using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.Data;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Models;
using FribergCarRentals.Helpers;
using FribergCarRentals.Areas.Administration.ViewModels;
using Microsoft.AspNetCore.Authorization;
using FribergCarRentals.Areas.CustomerCenter.ViewModels;
using FribergCarRentals.Areas.Administration.Helpers;

namespace FribergCarRentals.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administration")]
    public class IdentityUserController : Controller
    {
        private readonly IRepository<Admin> _adminRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public IdentityUserController(IRepository<Admin> adminRepository, IRepository<Customer> customerRepository, UserManager<IdentityUser> userManager)
        {
            _adminRepository = adminRepository;
            _customerRepository = customerRepository;
            _userManager = userManager;
        }

        // GET: IdentityUserIndexViewModels
        public async Task<IActionResult> Index()
        {
            List<IdentityUserIndexViewModel> identityUserIndexViewModels = new();
            IEnumerable<IdentityUser> users = await _userManager.Users.ToListAsync();
            IEnumerable<Admin> admins = await _adminRepository.GetAll();
            IEnumerable<Customer> customers = await _customerRepository.GetAll();
            foreach (IdentityUser user in users)
            {
                IdentityUserIndexViewModel identityUserIndexViewModel = new()
                {
                    IdentityUserId = user.Id,
                    IdentityUserUsername = user.UserName,
                    IsAdmin = await _userManager.IsInRoleAsync(user, "Admin"),
                    IsCustomer = await _userManager.IsInRoleAsync(user, "Customer"),
                };
                if (identityUserIndexViewModel.IsAdmin)
                {
                    Admin? admin = admins.Where(a => a.IdentityUser.Id == user.Id).FirstOrDefault();

                    identityUserIndexViewModel.AdminId = admin.Id;
                    identityUserIndexViewModel.AdminName = admin.Id.ToString();
                }
                if (identityUserIndexViewModel.IsCustomer)
                {
                    Customer? customer = customers.Where(c => c.IdentityUser.Id == user.Id).FirstOrDefault();
                    identityUserIndexViewModel.CustomerId = customer.Id;
                    identityUserIndexViewModel.CustomerName = $"{customer.FirstName} {customer.LastName}";
                }
                identityUserIndexViewModels.Add(identityUserIndexViewModel);
            }
            return View(identityUserIndexViewModels);
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
        public async Task<IActionResult> Create([Bind("Username")] IdentityUserViewModel identityUserViewModel)
        {
            if (RoleValidationHelper.EmailAlreadyClaimed(identityUserViewModel.Username, _userManager))
            {
                return View(identityUserViewModel);
            }
            if (ModelState.IsValid)
            {
                IdentityUser identityUser = new IdentityUser() { UserName = identityUserViewModel.Username, Email = identityUserViewModel.Username };
                string initialPassword = "Abc123!";
                await _userManager.CreateAsync(identityUser, initialPassword);
                await _userManager.AddToRoleAsync(identityUser, "User");
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

            IdentityUser? user = await _userManager.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }

            IdentityUserEditViewModel identityUserEditViewModel = ViewModelMappingHelper.GetIdentityUserEditViewModel(user);
            return View(identityUserEditViewModel);
        }

        // POST: IdentityUserViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdentityUserId,IdentityUserUsername")] IdentityUserEditViewModel identityUserEditViewModel)
        {
            if (id != identityUserEditViewModel.IdentityUserId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(identityUserEditViewModel);
            }

            try
            {
                IdentityUser user = await ViewModelMappingHelper.GetIdentityUser(identityUserEditViewModel, _userManager);
                await _userManager.SetUserNameAsync(user, identityUserEditViewModel.IdentityUserUsername);
                // await _userManager.UpdateAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IdentityUser? identityUser = await _userManager.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (identityUser == null)
            {
                return NotFound();
            }

            IdentityUserDeleteViewModel identityUserDeleteViewModel = ViewModelMappingHelper.GetIdentityUserDeleteViewModel(identityUser);
            return View(identityUserDeleteViewModel);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            IEnumerable<Admin> admins = await _adminRepository.GetAll();
            Admin? admin = admins.Where(a => a.IdentityUser.Id == id).FirstOrDefault();
            if (admin != null)
            {
                await _adminRepository.Delete(admin.Id);
            }

            IEnumerable<Customer> customers = await _customerRepository.GetAll();
            Customer? customer = customers.Where(c => c.IdentityUser.Id == id).FirstOrDefault();
            if (customer != null)
            {
                await _customerRepository.Delete(customer.Id);
            }

            IdentityUser? identityUser = await _userManager.Users.Where(iu => iu.Id == id).FirstOrDefaultAsync();
            if (identityUser != null)
            {
                await _userManager.DeleteAsync(identityUser);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
