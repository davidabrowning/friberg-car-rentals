using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.Data;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Authorization;
using FribergCarRentals.Helpers;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Areas.Administration.ViewModels;
using FribergCarRentals.Areas.Administration.Helpers;

namespace FribergCarRentals.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administration")]
    public class CustomerController : Controller
    {
        private readonly ICustomer _customerRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public CustomerController(ICustomer customerRepository, UserManager<IdentityUser> userManager)
        {
            _customerRepository = customerRepository;
            _userManager = userManager;
        }

        // GET: Customer
        public async Task<IActionResult> Index()
        {
            List<CustomerViewModel> customerViewModels = new();
            IEnumerable<Customer> customers = _customerRepository.GetAll();
            foreach (Customer customer in customers)
            {
                CustomerViewModel customerViewModel = ViewModelMappingHelper.GetCustomerViewModel(customer);
                customerViewModels.Add(customerViewModel);
            }
            return View(customerViewModels);
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer customer = _customerRepository.GetById((int)id);
            if (customer == null)
            {
                return NotFound();
            }

            CustomerViewModel customerViewModel = ViewModelMappingHelper.GetCustomerViewModel(customer);
            return View(customerViewModel);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,HomeCity,HomeCountry")] CustomerViewModel customerViewModel)
        {
            if (RoleValidationHelper.EmailAlreadyClaimed(customerViewModel.Email, _userManager))
            {
                return View(customerViewModel);
            }
            if (ModelState.IsValid)
            {
                IdentityUser identityUser = new IdentityUser(){ UserName = customerViewModel.Email, Email = customerViewModel.Email };
                string initialPassword = "Abc123!";
                List<Reservation> reservations = new();
                Customer customer = ViewModelMappingHelper.GetCustomer(customerViewModel, identityUser, reservations);
                await _userManager.CreateAsync(identityUser, initialPassword);
                await _userManager.AddToRoleAsync(identityUser, "User");
                _customerRepository.Add(customer);
                return RedirectToAction(nameof(Index));
            }
            return View(customerViewModel);
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer customer = _customerRepository.GetById((int)id);
            if (customer == null)
            {
                return NotFound();
            }

            CustomerViewModel customerViewModel = ViewModelMappingHelper.GetCustomerViewModel(customer);
            return View(customerViewModel);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,HomeCity,HomeCountry")] CustomerViewModel customerViewModel)
        {
            if (id != customerViewModel.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Customer customer = _customerRepository.GetById(customerViewModel.CustomerId);
                    customer = ViewModelMappingHelper.GetCustomer(customerViewModel, customer.IdentityUser, customer.Reservations);
                    _customerRepository.Update(customer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customerViewModel.CustomerId))
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
            return View(customerViewModel);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer customer = _customerRepository.GetById((int)id);
            if (customer == null)
            {
                return NotFound();
            }

            CustomerViewModel customerViewModel = ViewModelMappingHelper.GetCustomerViewModel(customer);
            return View(customerViewModel);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _customerRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _customerRepository.IdExists(id);
        }
    }
}
