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

namespace FribergCarRentals.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customer
        public async Task<IActionResult> Index()
        {
            List<CustomerViewModel> customerViewModels = new();
            List<Customer> customers = await _context.Customers.Include(c => c.ApplicationUser).ToListAsync();
            foreach (Customer customer in customers)
            {
                CustomerViewModel customerViewModel = new();
                customerViewModel.Id = customer.Id;
                customerViewModel.FirstName = customer.FirstName;
                customerViewModel.LastName = customer.LastName;
                customerViewModel.HomeCity = customer.HomeCity;
                customerViewModel.HomeCountry = customer.HomeCountry;
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
            customerViewModel.Id = customer.Id;
            customerViewModel.FirstName = customer.FirstName;
            customerViewModel.LastName = customer.LastName;
            customerViewModel.HomeCity = customer.HomeCity;
            customerViewModel.HomeCountry = customer.HomeCountry;
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
                customer.FirstName = customerViewModel.FirstName;
                customer.LastName = customerViewModel.LastName;
                customer.HomeCity = customerViewModel.HomeCity;
                customer.HomeCountry = customerViewModel.HomeCountry;
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

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            CustomerViewModel customerViewModel = new();
            customerViewModel.Id = customer.Id;
            customerViewModel.FirstName = customer.FirstName;
            customerViewModel.LastName = customer.LastName;
            customer.HomeCity = customerViewModel.HomeCity;
            customer.HomeCountry = customerViewModel.HomeCountry;
            return View(customerViewModel);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName")] CustomerViewModel customerViewModel)
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
                    customer.FirstName = customerViewModel.FirstName;
                    customer.LastName = customerViewModel.LastName;
                    customer.HomeCity = customerViewModel.HomeCity;
                    customer.HomeCountry = customerViewModel.HomeCountry;
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
            customerViewModel.Id = customer.Id;
            customerViewModel.FirstName = customer.FirstName;
            customerViewModel.LastName = customer.LastName;
            customerViewModel.HomeCity = customer.HomeCity;
            customerViewModel.HomeCountry = customer.HomeCountry;
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
