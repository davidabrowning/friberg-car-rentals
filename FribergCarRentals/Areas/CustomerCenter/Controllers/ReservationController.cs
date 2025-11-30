using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.Mvc.Areas.CustomerCenter.Views.Reservation;
using FribergCarRentals.Mvc.Attributes;
using FribergCarRentals.Mvc.Session;
using FribergCarRentals.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Mvc.Areas.CustomerCenter.Controllers
{
    [RequireCustomer]
    [Area("CustomerCenter")]
    public class ReservationController : Controller
    {
        private readonly ICRUDApiClient<CustomerDto> _customerDtoApiClient;
        private readonly ICRUDApiClient<ReservationDto> _reservationDtoApiClient;
        private readonly ICRUDApiClient<CarDto> _carDtoApiClient;
        private readonly IUserApiClient _userApiClient;
        private readonly UserSession _userSession;

        public ReservationController(ICRUDApiClient<CustomerDto> customerDtoApiClient,
            ICRUDApiClient<ReservationDto> reservationDtoApiClient,
            ICRUDApiClient<CarDto> carDtoApiClient,
            IUserApiClient userApiClient,
            UserSession userSession)
        {
            _customerDtoApiClient = customerDtoApiClient;
            _reservationDtoApiClient = reservationDtoApiClient;
            _carDtoApiClient = carDtoApiClient;
            _userApiClient = userApiClient;
            _userSession = userSession;
        }

        // GET: CustomerCenter/Reservation
        public async Task<IActionResult> Index()
        {
            UserDto userDto = _userSession.UserDto;
            if (userDto.UserId == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

            if (userDto.CustomerDto == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
            }

            List<IndexReservationViewModel> reservationIndexViewModelList = new();
            try
            {
                IEnumerable<ReservationDto> reservationDtos = await _reservationDtoApiClient.GetAsync();
                List<ReservationDto> thisCustomersReservationDtos = reservationDtos
                    .Where(r => r.CustomerDto.Id == userDto.CustomerDto.Id)
                    .OrderByDescending(r => r.StartDate)
                    .ToList();
                foreach (ReservationDto reservationDto in thisCustomersReservationDtos)
                {
                    IndexReservationViewModel reservationViewModel = new()
                    {
                        Id = reservationDto.Id,
                        StartDate = reservationDto.StartDate,
                        EndDate = reservationDto.EndDate,
                        CarDto = reservationDto.CarDto,
                    };
                    reservationIndexViewModelList.Add(reservationViewModel);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
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

            UserDto userDto = _userSession.UserDto;
            if (userDto.CustomerDto == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
            }

            try
            {
                CreateReservationViewModel reservationCreateViewModel = new()
                {
                    CustomerId = userDto.CustomerDto.Id,
                    PreselectedCarId = preselectedCarId,
                    CarDtos = await _carDtoApiClient.GetAsync()
                };
                return View(reservationCreateViewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: CustomerCenter/Reservation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReservationViewModel reservationCreateViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    reservationCreateViewModel.CarDtos = await _carDtoApiClient.GetAsync();
                    return View(reservationCreateViewModel);
                }

                CustomerDto? customerDto = await _customerDtoApiClient.GetAsync(reservationCreateViewModel.CustomerId);
                if (customerDto == null)
                {
                    TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                    return RedirectToAction("Index");
                }

                CarDto? carDto = await _carDtoApiClient.GetAsync(reservationCreateViewModel.CarId);
                if (carDto == null)
                {
                    TempData["ErrorMessage"] = UserMessage.ErrorCarIsNull;
                    return RedirectToAction("Index");
                }

                ReservationDto reservationDto = new()
                {
                    Id = 0,
                    StartDate = reservationCreateViewModel.StartDate,
                    EndDate = reservationCreateViewModel.EndDate,
                    CustomerDto = customerDto,
                    CarDto = carDto,
                };
                await _reservationDtoApiClient.PostAsync(reservationDto);

                TempData["SuccessMessage"] = $"{UserMessage.SuccessReservationCreated} Enjoy the {reservationDto.CarDto.Year} {reservationDto.CarDto.Make} {reservationDto.CarDto.Model}!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: CustomerCenter/Reservation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsNull;
                return RedirectToAction("Index");
            }

            try
            {
                ReservationDto? reservation = await _reservationDtoApiClient.GetAsync((int)id);
                if (reservation == null)
                {
                    TempData["ErrorMessage"] = UserMessage.ErrorReservationIsNull;
                    return RedirectToAction("Index");
                }

                UserDto userDto = _userSession.UserDto;
                if (userDto.CustomerDto == null)
                {
                    TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                    return RedirectToAction("Index");
                }

                if (reservation.CustomerDto.Id != userDto.CustomerDto.Id)
                {
                    TempData["ErrorMessage"] = UserMessage.ErrorUnauthorized;
                    return RedirectToAction("Index");
                }

                DeleteReservationViewModel reservationDeleteViewModel = new()
                {
                    Id = reservation.Id,
                    StartDate = reservation.StartDate,
                    EndDate = reservation.EndDate,
                    CarDto = reservation.CarDto,
                };

                return View(reservationDeleteViewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: CustomerCenter/Reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                ReservationDto? reservation = await _reservationDtoApiClient.GetAsync(id);
                if (reservation == null)
                {
                    TempData["ErrorMessage"] = UserMessage.ErrorReservationIsNull;
                    return RedirectToAction("Index");
                }

                await _reservationDtoApiClient.DeleteAsync(reservation.Id);

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
