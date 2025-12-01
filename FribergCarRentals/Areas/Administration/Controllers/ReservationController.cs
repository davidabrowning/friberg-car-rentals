using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.Mvc.Areas.Administration.Views.Reservation;
using FribergCarRentals.Mvc.Attributes;
using FribergCarRentals.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Mvc.Areas.Administration.Controllers
{
    [RequireAdmin]
    [Area("Administration")]
    public class ReservationController : Controller
    {
        private readonly ICRUDApiClient<ReservationDto> _reservationDtoApiClient;

        public ReservationController(ICRUDApiClient<ReservationDto> reservationDtoApiClient)
        {
            _reservationDtoApiClient = reservationDtoApiClient;
        }

        // GET: Reservation
        public async Task<IActionResult> Index()
        {
            List<IndexReservationViewModel> indexReservationViewModelList = new();
            try
            {
                IEnumerable<ReservationDto> reservationDtos = await _reservationDtoApiClient.GetAsync();
                foreach (ReservationDto reservationDto in reservationDtos)
                {
                    IndexReservationViewModel indexReservationViewModel = new IndexReservationViewModel()
                    {
                        Id = reservationDto.Id,
                        StartDate = reservationDto.StartDate,
                        EndDate = reservationDto.EndDate,
                        CarDto = reservationDto.CarDto,
                        CustomerDto = reservationDto.CustomerDto,
                    };
                    indexReservationViewModelList.Add(indexReservationViewModel);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
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

            try
            {
                ReservationDto? reservationDto = await _reservationDtoApiClient.GetAsync((int)id);
                if (reservationDto == null)
                {
                    TempData["ErrorMessage"] = UserMessage.ErrorReservationIsNull;
                    return RedirectToAction("Index");
                }

                CarDto carDto = new()
                {
                    Id = reservationDto.CarDto.Id,
                    Description = reservationDto.CarDto.Description,
                    Year = reservationDto.CarDto.Year,
                    Make = reservationDto.CarDto.Make,
                    Model = reservationDto.CarDto.Model,
                };
                CustomerDto customerDto = new()
                {
                    Id = reservationDto.CustomerDto.Id,
                    UserId = reservationDto.CustomerDto.UserId,
                    FirstName = reservationDto.CustomerDto.FirstName
                };

                DeleteReservationViewModel deleteReservationViewModel = new()
                {
                    Id = reservationDto.Id,
                    StartDate = reservationDto.StartDate,
                    EndDate = reservationDto.EndDate,
                    CarDto = carDto,
                    CustomerDto = customerDto,
                };
                return View(deleteReservationViewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Reservation/DeleteAsync/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
