using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Core.Models;
using Microsoft.AspNetCore.Authorization;
using FribergCarRentals.Areas.Administration.Views.Reservation;
using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.ApiClients;

namespace FribergCarRentals.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administration")]
    public class ReservationController : Controller
    {
        private readonly IApiClient<Reservation> _reservationApiClient;

        public ReservationController(IApiClient<Reservation> reservationApiClient)
        {
            _reservationApiClient = reservationApiClient;
        }

        // GET: Reservation
        public async Task<IActionResult> Index()
        {
            List<IndexReservationViewModel> indexReservationViewModelList = new();
            IEnumerable<Reservation> reservations = await _reservationApiClient.GetAsync();
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
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsNull;
                return RedirectToAction("Index");
            }

            Reservation? reservation = await _reservationApiClient.GetAsync((int)id);
            if (reservation == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorReservationIsNull;
                return RedirectToAction("Index");
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
            Reservation? reservation = await _reservationApiClient.GetAsync((int)id);
            if (reservation == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorReservationIsNull;
                return RedirectToAction("Index");
            }

            await _reservationApiClient.DeleteAsync(id);

            TempData["SuccessMessage"] = UserMessage.SuccessReservationDeleted;
            return RedirectToAction("Index");
        }
    }
}
