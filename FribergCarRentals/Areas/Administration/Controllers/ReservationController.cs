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
using FribergCarRentals.Areas.Administration.ViewModels;
using FribergCarRentals.Areas.Administration.Helpers;

namespace FribergCarRentals.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administration")]
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservation
        public async Task<IActionResult> Index()
        {
            List<ReservationViewModel> reservationViewModels = new();
            List<Reservation> reservations = await _context.Reservations.ToListAsync();
            foreach (Reservation reservation in reservations)
            {
                ReservationViewModel reservationViewModel = ViewModelMappingHelper.GetReservationViewModel(reservation);
                reservationViewModels.Add(reservationViewModel);
            }
            return View(reservationViewModels);
        }

        // GET: Reservation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reservation reservation = await _context.Reservations.FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            ReservationViewModel reservationViewModel = ViewModelMappingHelper.GetReservationViewModel(reservation);
            return View(reservationViewModel);
        }

        // GET: Reservation/Create
        public async Task<IActionResult> Create()
        {
            ReservationViewModel reservationViewModel = new();
            reservationViewModel.AllCars = await _context.Cars.ToListAsync();
            reservationViewModel.AllCustomers = await _context.Customers.ToListAsync();
            return View();
        }

        // POST: Reservation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartDate,EndDate,Car,Customer")] ReservationViewModel reservationViewModel)
        {
            if (ModelState.IsValid)
            {
                Reservation reservation = ViewModelMappingHelper.GetReservation(reservationViewModel);
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reservationViewModel);
        }

        // GET: Reservation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            ReservationViewModel reservationViewModel = ViewModelMappingHelper.GetReservationViewModel(reservation);
            return View(reservationViewModel);
        }

        // POST: Reservation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartDate,EndDate")] ReservationViewModel reservationViewModel)
        {
            if (id != reservationViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Reservation reservation = ViewModelMappingHelper.GetReservation(reservationViewModel);
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservationViewModel.Id))
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
            return View(reservationViewModel);
        }

        // GET: Reservation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            ReservationViewModel reservationViewModel = ViewModelMappingHelper.GetReservationViewModel(reservation);
            return View(reservationViewModel);
        }

        // POST: Reservation/DeleteAsync/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
