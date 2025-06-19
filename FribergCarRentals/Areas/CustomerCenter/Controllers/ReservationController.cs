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
using FribergCarRentals.Areas.CustomerCenter.ViewModels;
using FribergCarRentals.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FribergCarRentals.Areas.CustomerCenter.Controllers
{
    [Authorize(Roles = "Customer")]
    [Area("CustomerCenter")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly ICarService _carService;
        private readonly IUserService _userService;

        public ReservationController(ApplicationDbContext context, IReservationService reservationService, ICarService carService, IUserService userService)
        {
            _reservationService = reservationService;
            _carService = carService;
            _userService = userService;
        }

        // GET: CustomerCenter/Reservation
        public async Task<IActionResult> Index()
        {
            IdentityUser? identityUser = await _userService.GetCurrentUser();
            if (identityUser == null)
            {
                return NotFound();
            }

            Customer? customer = await _userService.GetCustomerByUserAsync(identityUser);
            if (customer == null)
            {
                return NotFound();
            }

            List<ReservationIndexViewModel> reservationIndexViewModelList = new();
            foreach (Reservation reservation in customer.Reservations)
            {
                ReservationIndexViewModel reservationViewModel = new()
                {
                    Id = reservation.Id,
                    StartDate = reservation.StartDate,
                    EndDate = reservation.EndDate,
                    Car = reservation.Car,
                };
                reservationIndexViewModelList.Add(reservationViewModel);
            }
            
            return View(reservationIndexViewModelList);
        }

        // GET: CustomerCenter/Reservation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reservation? reservation = await _reservationService.GetByIdAsync((int)id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: CustomerCenter/Reservation/Create/5
        public async Task<IActionResult> Create(int? id)
        {
            int preselectedCarId = 0;
            if (id != null)
            {
                preselectedCarId = (int)id;
            }
            IdentityUser identityUser = await _userService.GetCurrentUser();
            Customer customer = await _userService.GetCustomerByUserAsync(identityUser);
            ReservationCreateViewModel reservationCreateViewModel = new()
            {
                CustomerId = customer.Id,
                PreselectedCarId = preselectedCarId,
                Cars = await _carService.GetAllAsync(),
            };
            return View(reservationCreateViewModel);
        }

        // POST: CustomerCenter/Reservation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationCreateViewModel reservationCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                reservationCreateViewModel.Cars = await _carService.GetAllAsync();
                return View(reservationCreateViewModel);
            }

            Customer? customer = await _userService.GetCustomerByIdAsync(reservationCreateViewModel.CustomerId);
            Car? car = await _carService.GetByIdAsync(reservationCreateViewModel.CarId);
            if (customer == null || car == null) 
            {
                return NotFound();
            }

            Reservation reservation = new()
            {
                StartDate = reservationCreateViewModel.StartDate,
                EndDate = reservationCreateViewModel.EndDate,
                Customer = customer,
                Car = car,
            };
            await _reservationService.CreateAsync(reservation);

            return RedirectToAction(nameof(Index));
        }

        // GET: CustomerCenter/Reservation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reservation? reservation = await _reservationService.GetByIdAsync((int)id);
            if (reservation == null)
            {
                return NotFound();
            }

            ReservationDeleteViewModel reservationDeleteViewModel = new()
            {
                Id = reservation.Id,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                Car = reservation.Car,
            };

            return View(reservationDeleteViewModel);
        }

        // POST: CustomerCenter/Reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Reservation? reservation = await _reservationService.GetByIdAsync(id);
            if (reservation != null)
            {
                await _reservationService.DeleteAsync(reservation.Id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
