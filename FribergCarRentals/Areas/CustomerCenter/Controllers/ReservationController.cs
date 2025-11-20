using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Core.Models;
using Microsoft.AspNetCore.Authorization;
using FribergCarRentals.Areas.CustomerCenter.Views.Reservation;
using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.Core.Interfaces.Services;

namespace FribergCarRentals.Areas.CustomerCenter.Controllers
{
    [Authorize(Roles = "Customer")]
    [Area("CustomerCenter")]
    public class ReservationController : Controller
    {
        private readonly IApiClient<Reservation> _reservationApiClient;
        private readonly ICarService _carService;
        private readonly IUserService _userService;

        public ReservationController(IApiClient<Reservation> reservationApiClient, ICarService carService, IUserService userService)
        {
            _reservationApiClient = reservationApiClient;
            _carService = carService;
            _userService = userService;
        }

        // GET: CustomerCenter/Reservation
        public async Task<IActionResult> Index()
        {
            string? userId = await _userService.GetCurrentUserId();
            if (userId == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

            Customer? customer = await _userService.GetCustomerByUserIdAsync(userId);
            if (customer == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
            }

            List<IndexReservationViewModel> reservationIndexViewModelList = new();
            foreach (Reservation reservation in customer.Reservations.OrderByDescending(r => r.StartDate).ToList())
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

            string? userId = await _userService.GetCurrentUserId();
            if (userId == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

            Customer? customer = await _userService.GetCustomerByUserIdAsync(userId);
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

            Customer? customer = await _userService.GetCustomerByCustomerIdAsync(reservationCreateViewModel.CustomerId);
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
            await _reservationApiClient.PostAsync(reservation);

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

            Reservation? reservation = await _reservationApiClient.GetAsync((int)id);
            if (reservation == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorReservationIsNull;
                return RedirectToAction("Index");
            }

            Customer? currentCustomer = await _userService.GetSignedInCustomer();
            if (currentCustomer == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
            }

            if (reservation.Customer != currentCustomer)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorAccessDenied;
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
            Reservation? reservation = await _reservationApiClient.GetAsync(id);
            if (reservation == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorReservationIsNull;
                return RedirectToAction("Index");
            }

            await _reservationApiClient.DeleteAsync(reservation.Id);

            TempData["SuccessMessage"] = UserMessage.SuccessReservationDeleted;
            return RedirectToAction("Index");
        }
    }
}
