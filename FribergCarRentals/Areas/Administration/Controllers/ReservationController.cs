﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Authorization;
using FribergCarRentals.Interfaces;
using FribergCarRentals.Areas.Administration.Views.Reservation;

namespace FribergCarRentals.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administration")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        // GET: Reservation
        public async Task<IActionResult> Index()
        {
            List<IndexReservationViewModel> indexReservationViewModelList = new();
            IEnumerable<Reservation> reservations = await _reservationService.GetAllAsync();
            foreach (Reservation reservation in reservations)
            {
                IndexReservationViewModel indexReservationViewModel = new IndexReservationViewModel()
                {
                    Id = reservation.Id,
                    StartDate = reservation.StartDate,
                    EndDate = reservation.EndDate,
                    Car = reservation.Car,
                    Customer = reservation.Customer,
                };
                indexReservationViewModelList.Add(indexReservationViewModel);
            }
            return View(indexReservationViewModelList);
        }

        // GET: Reservation/Delete/5
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

            DeleteReservationViewModel deleteReservationViewModel = new()
            {
                Id = reservation.Id,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                Car = reservation.Car,
                Customer = reservation.Customer,
            };
            return View(deleteReservationViewModel);
        }

        // POST: Reservation/DeleteAsync/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Reservation? reservation = await _reservationService.GetByIdAsync((int)id);
            if (reservation != null)
            {
                await _reservationService.DeleteAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
