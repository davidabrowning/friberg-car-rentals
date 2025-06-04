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

        // GET: IdentityUserViewModels
        public async Task<IActionResult> Index()
        {
            List<IdentityUserViewModel> identityUserViewModels = new();
            List<IdentityUser> users = _userManager.Users.ToList();
            IEnumerable<Admin> admins = await _adminRepository.GetAll();
            IEnumerable<Customer> customers = await _customerRepository.GetAll();
            foreach (IdentityUser user in users)
            {
                int adminId = -1;
                Admin? admin = admins.Where(a => a.IdentityUser.Id == user.Id).FirstOrDefault();
                if (admin != null)
                    adminId = admin.Id;
                int customerId = -1;
                string customerFirstName = "";
                string customerLastName = "";
                Customer? customer = customers.Where(c => c.IdentityUser.Id == user.Id).FirstOrDefault();
                if (customer != null)
                {
                    customerId = customer.Id;
                    customerFirstName = customer.FirstName;
                    customerLastName = customer.LastName;
                }
                IdentityUserViewModel identityUserViewModel = new()
                {
                    Id = user.Id,
                    Username = user.UserName,
                    IsAdmin = await _userManager.IsInRoleAsync(user, "Admin"),
                    IsCustomer = await _userManager.IsInRoleAsync(user, "Customer"),
                    IsUser = await _userManager.IsInRoleAsync(user, "User"),
                    AdminId = adminId,
                    CustomerId = customerId,
                    CustomerFirstName = customerFirstName,
                    CustomerLastName = customerLastName,
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

            IdentityUser user = await _userManager.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }

            IdentityUserViewModel identityUserViewModel = await ViewModelMappingHelper.GetIdentityUserViewModel(user, _userManager);
            return View(identityUserViewModel);
        }

        // POST: IdentityUserViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Username")] IdentityUserViewModel identityUserViewModel)
        {
            if (id != identityUserViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    IdentityUser user = await ViewModelMappingHelper.GetIdentityUser(identityUserViewModel, _userManager);
                    await _userManager.UpdateAsync(user);
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
