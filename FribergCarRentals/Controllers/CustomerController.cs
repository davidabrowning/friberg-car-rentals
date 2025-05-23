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
using FribergCarRentals.ViewModels;
using FribergCarRentals.Helpers;
using Microsoft.AspNetCore.Identity;

namespace FribergCarRentals.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Customer
        public async Task<IActionResult> Index()
        {
            List<CustomerViewModel> customerViewModels = new();
            List<Customer> customers = await _context.Customers.Include(c => c.ApplicationUser).ToListAsync();
            foreach (Customer customer in customers)
            {
                CustomerViewModel customerViewModel = new();
                ViewModelMappingHelper.MapAToB(customer, customerViewModel);
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

            Customer customer = await _context.Customers.FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            CustomerViewModel customerViewModel = new();
            ViewModelMappingHelper.MapAToB(customer, customerViewModel);
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
        public async Task<IActionResult> Create([Bind("FirstName,LastName,HomeCity,HomeCountry")] CustomerViewModel customerViewModel)
        {
            if (ModelState.IsValid)
            {
                Customer customer = new();
                ViewModelMappingHelper.MapAToB(customerViewModel, customer);
                _context.Add(customer);
                await _context.SaveChangesAsync();
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

            var customer = await _context.Customers.Include(c => c.ApplicationUser).FirstOrDefaultAsync(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            CustomerViewModel customerViewModel = new();
            ViewModelMappingHelper.MapAToB(customer, customerViewModel);
            List<ApplicationUser> applicationUsers = await _userManager.Users.ToListAsync();
            customerViewModel.ApplicationUsers = applicationUsers.Select(au => new SelectListItem()
            {
                Value = au.Id,
                Text = au.UserName
            }).ToList();
            return View(customerViewModel);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,HomeCity,HomeCountry")] CustomerViewModel customerViewModel)
        {
            if (id != customerViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Customer customer = await _context.Customers.Where(c => c.Id == customerViewModel.Id).FirstOrDefaultAsync();
                    ViewModelMappingHelper.MapAToB(customerViewModel, customer);
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customerViewModel.Id))
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

            Customer? customer = await _context.Customers.FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            CustomerViewModel customerViewModel = new();
            ViewModelMappingHelper.MapAToB(customer, customerViewModel);
            return View(customerViewModel);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
