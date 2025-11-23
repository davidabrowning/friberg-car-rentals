using FribergCarRentals.Areas.CustomerCenter.Views.Reservation;
using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;
using FribergCarRentals.WebApi.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Areas.CustomerCenter.Controllers
{
    [Authorize(Roles = "Customer")]
    [Area("CustomerCenter")]
    public class ReservationController : Controller
    {
        private readonly ICRUDApiClient<CustomerDto> _customerDtoApiClient;
        private readonly ICRUDApiClient<ReservationDto> _reservationDtoApiClient;
        private readonly ICRUDApiClient<CarDto> _carDtoApiClient;
        private readonly IAuthApiClient _authApiClient;

        public ReservationController(ICRUDApiClient<CustomerDto> customerDtoApiClient, ICRUDApiClient<ReservationDto> reservationDtoApiClient, ICRUDApiClient<CarDto> carDtoApiClient, IAuthApiClient authApiClient)
        {
            _customerDtoApiClient = customerDtoApiClient;
            _reservationDtoApiClient = reservationDtoApiClient;
            _carDtoApiClient = carDtoApiClient;
            _authApiClient = authApiClient;
        }

        // GET: CustomerCenter/Reservation
        public async Task<IActionResult> Index()
        {
            string? userId = await _authApiClient.GetCurrentSignedInUserIdAsync();
            if (userId == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

            if (! await _authApiClient.IsCustomerAsync(userId))
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
            }

            int customerId = await _authApiClient.GetCustomerIdByUserId(userId);
            CustomerDto? customerDto = await _customerDtoApiClient.GetAsync(customerId);
            if (customerDto == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
            }

            IEnumerable<ReservationDto> reservationDtos = await _reservationDtoApiClient.GetAsync();
            List<ReservationDto> thisCustomersReservationDtos = reservationDtos
                .Where(r => r.CustomerDto.Id == customerDto.Id)
                .OrderByDescending(r => r.StartDate)
                .ToList();
            List<IndexReservationViewModel> reservationIndexViewModelList = new();
            foreach (ReservationDto reservationDto in thisCustomersReservationDtos)
            {
                IndexReservationViewModel reservationViewModel = new()
                {
                    Id = reservationDto.Id,
                    StartDate = reservationDto.StartDate,
                    EndDate = reservationDto.EndDate,
                    // Car = reservationDto.Car,
                    Car = new()
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

            string? userId = await _authApiClient.GetCurrentSignedInUserIdAsync();
            if (userId == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

            int customerId = await _authApiClient.GetCustomerIdByUserId(userId);
            CustomerDto? customerDto = await _customerDtoApiClient.GetAsync(customerId);
            if (customerDto == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
            }

            CreateReservationViewModel reservationCreateViewModel = new()
            {
                CustomerId = customerDto.Id,
                PreselectedCarId = preselectedCarId,
                Cars = CarMapper.ToModels(await _carDtoApiClient.GetAsync())
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
                reservationCreateViewModel.Cars = CarMapper.ToModels(await _carDtoApiClient.GetAsync());
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

            TempData["SuccessMessage"] = UserMessage.SuccessReservationCreated + " " + reservationDto.ToString();
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

            ReservationDto? reservation = await _reservationDtoApiClient.GetAsync((int)id);
            if (reservation == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorReservationIsNull;
                return RedirectToAction("Index");
            }

            string? signedInUserId = await _authApiClient.GetCurrentSignedInUserIdAsync();
            if (signedInUserId == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }
            int customerId = await _authApiClient.GetCustomerIdByUserId(signedInUserId);
            CustomerDto? signedInCustomerDto = await _customerDtoApiClient.GetAsync(customerId);
            if (signedInCustomerDto == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCustomerIsNull;
                return RedirectToAction("Index");
            }

            if (reservation.CustomerDto.Id != signedInCustomerDto.Id)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorAccessDenied;
                return RedirectToAction("Index");
            }

            DeleteReservationViewModel reservationDeleteViewModel = new()
            {
                Id = reservation.Id,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                Car = CarMapper.ToModel(reservation.CarDto),
            };

            return View(reservationDeleteViewModel);
        }

        // POST: CustomerCenter/Reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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
    }
}
