using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Core.Models;
using Microsoft.AspNetCore.Authorization;
using FribergCarRentals.Areas.Administration.Views.Reservation;
using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administration")]
    public class ReservationController : Controller
    {
        private readonly IApiClient<ReservationDto> _reservationDtoApiClient;

        public ReservationController(IApiClient<ReservationDto> reservationDtoApiClient)
        {
            _reservationDtoApiClient = reservationDtoApiClient;
        }

        // GET: Reservation
        public async Task<IActionResult> Index()
        {
            List<IndexReservationViewModel> indexReservationViewModelList = new();
            IEnumerable<ReservationDto> reservationDtos = await _reservationDtoApiClient.GetAsync();
            foreach (ReservationDto reservationDto in reservationDtos)
            {
                Core.Models.Car car = new()
                {
                    Id = reservationDto.CarDto.Id,
                    Description = "Complete in ReservationController"
                };
                Customer customer = new()
                {
                    Id = reservationDto.CustomerDto.Id,
                    UserId = reservationDto.CustomerDto.UserId,
                    FirstName = "Complete in ReservationController"
                };
                IndexReservationViewModel indexReservationViewModel = new IndexReservationViewModel()
                {
                    Id = reservationDto.Id,
                    StartDate = reservationDto.StartDate,
                    EndDate = reservationDto.EndDate,
                    Car = car,
                    Customer = customer,
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

            ReservationDto? reservationDto = await _reservationDtoApiClient.GetAsync((int)id);
            if (reservationDto == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorReservationIsNull;
                return RedirectToAction("Index");
            }

            Core.Models.Car car = new()
            {
                Id = reservationDto.CarDto.Id,
                Description = "Complete in ReservationController"
            };
            Customer customer = new()
            {
                Id = reservationDto.CustomerDto.Id,
                UserId = reservationDto.CustomerDto.UserId,
                FirstName = "Complete in ReservationController"
            };

            DeleteReservationViewModel deleteReservationViewModel = new()
            {
                Id = reservationDto.Id,
                StartDate = reservationDto.StartDate,
                EndDate = reservationDto.EndDate,
                Car = car,
                Customer = customer,
            };
            return View(deleteReservationViewModel);
        }

        // POST: Reservation/DeleteAsync/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ReservationDto? reservation = await _reservationDtoApiClient.GetAsync((int)id);
            if (reservation == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorReservationIsNull;
                return RedirectToAction("Index");
            }

            await _reservationDtoApiClient.DeleteAsync(id);

            TempData["SuccessMessage"] = UserMessage.SuccessReservationDeleted;
            return RedirectToAction("Index");
        }
    }
}
