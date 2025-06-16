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
using FribergCarRentals.Areas.Administration.ViewModels;
using Microsoft.AspNetCore.Authorization;
using FribergCarRentals.Areas.Administration.Helpers;
using FribergCarRentals.Interfaces;

namespace FribergCarRentals.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administration")]
    public class IdentityUserController : Controller
    {
        private readonly IRepository<Admin> _adminRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IIdentityUserService _identityUserService;

        public IdentityUserController(IRepository<Admin> adminRepository, IRepository<Customer> customerRepository, IIdentityUserService identityUserService)
        {
            _adminRepository = adminRepository;
            _customerRepository = customerRepository;
            _identityUserService = identityUserService;
        }

        // GET: IdentityUserIndexViewModels
        public async Task<IActionResult> Index()
        {
            List<IdentityUserIndexViewModel> viewModelList = await CreateIdentityUserIndexViewModels();
            return View(viewModelList);
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
        public async Task<IActionResult> Create(IdentityUserViewModel identityUserViewModel)
        {
            if (await IsAlreadyTaken(identityUserViewModel.Username))
            {
                return View(identityUserViewModel);
            }

            if (!ModelState.IsValid)
            {
                return View(identityUserViewModel);
            }

            await _identityUserService.AddIdentityUserAsync(identityUserViewModel.Username);

            return RedirectToAction(nameof(Index));
        }

        // GET: IdentityUserViewModels/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IdentityUser? identityUser = await _identityUserService.GetByIdAsync(id);
            if (identityUser is null)
            {
                return NotFound();
            }

            IdentityUserEditViewModel viewModel = ViewModelMakerHelper.MakeIdentityUserEditViewModel(identityUser);
            return View(viewModel);
        }

        // POST: IdentityUserViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, IdentityUserEditViewModel identityUserEditViewModel)
        {
            if (id != identityUserEditViewModel.IdentityUserId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(identityUserEditViewModel);
            }

            await _identityUserService.UpdateUsernameAsync(id, identityUserEditViewModel.IdentityUserUsername);

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IdentityUser? identityUser = await _identityUserService.GetByIdAsync(id);
            if (identityUser == null)
            {
                return NotFound();
            }

            IdentityUserDeleteViewModel identityUserDeleteViewModel = ViewModelMakerHelper.MakeIdentityUserDeleteViewModel(identityUser);
            return View(identityUserDeleteViewModel);
        }

        // POST: Admin/DeleteAsync/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _identityUserService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> IsAlreadyTaken(string identityUserUsername)
        {
            return await _identityUserService.UsernameExistsAsync(identityUserUsername);
        }

        private async Task<List<IdentityUserIndexViewModel>> CreateIdentityUserIndexViewModels()
        {
            List<IdentityUserIndexViewModel> identityUserIndexViewModels = new();
            IEnumerable<IdentityUser> users = await _identityUserService.GetAllAsync();
            foreach (IdentityUser user in users)
            {
                identityUserIndexViewModels.Add(await CreateIdentityUserIndexViewModel(user));
            }
            return identityUserIndexViewModels;
        }

        private async Task<IdentityUserIndexViewModel> CreateIdentityUserIndexViewModel(IdentityUser user)
        {
            IdentityUserIndexViewModel identityUserIndexViewModel = new()
            {
                IdentityUserId = user.Id,
                IdentityUserUsername = user.UserName,
                IsAdmin = await _identityUserService.IsInRoleAsync(user, "Admin"),
                IsCustomer = await _identityUserService.IsInRoleAsync(user, "Customer"),
            };
            if (identityUserIndexViewModel.IsAdmin)
            {
                IEnumerable<Admin> admins = await _adminRepository.GetAllAsync();
                Admin? admin = admins.Where(a => a.IdentityUser.Id == user.Id).FirstOrDefault();
                identityUserIndexViewModel.AdminId = admin.Id;
                identityUserIndexViewModel.AdminName = admin.Id.ToString();
            }
            if (identityUserIndexViewModel.IsCustomer)
            {
                IEnumerable<Customer> customers = await _customerRepository.GetAllAsync();
                Customer? customer = customers.Where(c => c.IdentityUser.Id == user.Id).FirstOrDefault();
                identityUserIndexViewModel.CustomerId = customer.Id;
                identityUserIndexViewModel.CustomerName = $"{customer.FirstName} {customer.LastName}";
            }
            return identityUserIndexViewModel;
        }
    }
}
