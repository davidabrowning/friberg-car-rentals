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
using FribergCarRentals.Interfaces;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Areas.CustomerCenter.Views.Reservation;
using FribergCarRentals.Helpers;

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
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

            Customer? customer = await _userService.GetCustomerByUserAsync(identityUser);
            if (customer == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
            }

            List<IndexReservationViewModel> reservationIndexViewModelList = new();
            foreach (Reservation reservation in customer.Reservations)
            {
                IndexReservationViewModel reservationViewModel = new()
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

        // GET: CustomerCenter/Reservation/Create/5
        public async Task<IActionResult> Create(int? id)
        {
            int preselectedCarId = 0;
            if (id != null)
            {
                preselectedCarId = (int)id;
            }

            IdentityUser? identityUser = await _userService.GetCurrentUser();
            if (identityUser == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

            Customer? customer = await _userService.GetCustomerByUserAsync(identityUser);
            if (customer == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
            }

            CreateReservationViewModel reservationCreateViewModel = new()
            {
                CustomerId = customer.Id,
                PreselectedCarId = preselectedCarId,
                Cars = await _carService.GetAllAsync(),
            };
            return View(reservationCreateViewModel);
        }

        // POST: CustomerCenter/Reservation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReservationViewModel reservationCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                reservationCreateViewModel.Cars = await _carService.GetAllAsync();
                return View(reservationCreateViewModel);
            }

            Customer? customer = await _userService.GetCustomerByIdAsync(reservationCreateViewModel.CustomerId);
            if (customer == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
            }

            Car? car = await _carService.GetByIdAsync(reservationCreateViewModel.CarId);
            if (car == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCarIsNull;
                return RedirectToAction("Index");
            }

            Reservation reservation = new()
            {
                StartDate = reservationCreateViewModel.StartDate,
                EndDate = reservationCreateViewModel.EndDate,
                Customer = customer,
                Car = car,
            };
            await _reservationService.CreateAsync(reservation);

            TempData["SuccessMessage"] = UserMessage.SuccessReservationCreated + " " + reservation.ToString();
            return RedirectToAction("Index");
        }

        // GET: CustomerCenter/Reservation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsNull;
                return RedirectToAction("Index");
            }

            Reservation? reservation = await _reservationService.GetByIdAsync((int)id);
            if (reservation == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorReservationIsNull;
                return RedirectToAction("Index");
            }

            DeleteReservationViewModel reservationDeleteViewModel = new()
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
            if (reservation == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorReservationIsNull;
                return RedirectToAction("Index");
            }

            await _reservationService.DeleteAsync(reservation.Id);

            TempData["SuccessMessage"] = UserMessage.SuccessReservationDeleted;
            return RedirectToAction("Index");
        }
    }
}
