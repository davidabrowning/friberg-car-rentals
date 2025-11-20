using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Core.Models;
using Microsoft.AspNetCore.Authorization;
using FribergCarRentals.Areas.Administration.Views.Car;
using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Interfaces.ApiClients;

namespace FribergCarRentals.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administration")]
    public class CarController : Controller
    {
        private readonly IApiClient<Car> _carApiClient;

        public CarController(IApiClient<Car> carApiClient)
        {
            _carApiClient = carApiClient;
        }

        // GET: Car
        public async Task<IActionResult> Index()
        {
            List<IndexCarViewModel> indexCarViewModelList = new();
            IEnumerable<Car> cars = await _carApiClient.GetAsync();
            foreach (Car car in cars)
            {
                IndexCarViewModel indexCarViewModel = new IndexCarViewModel()
                {
                    Id = car.Id,
                    Make = car.Make,
                    Model = car.Model,
                    Year = car.Year,
                    Description = car.Description,
                    ReservationIds = car.Reservations.Select(r => r.Id).ToList(),
                };
                indexCarViewModelList.Add(indexCarViewModel);
            }
            return View(indexCarViewModelList);
        }

        // GET: Car/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsNull;
                return RedirectToAction("Index");
            }

            Car? car = await _carApiClient.GetAsync((int)id);
            if (car == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCarIsNull;
                return RedirectToAction("Index");
            }

            DetailsCarViewModel detailsCarViewModel = new DetailsCarViewModel()
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                Description = car.Description,
                PhotoUrls = car.PhotoUrls,
                ReservationIds = car.Reservations.Select(r => r.Id).ToList(),
            };
            return View(detailsCarViewModel);
        }

        // GET: Car/Create
        public IActionResult Create()
        {
            CreateCarViewModel createCarViewModel = new();
            return View(createCarViewModel);
        }

        // POST: Car/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCarViewModel createCarViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createCarViewModel);
            }

            Car car = new()
            {
                Make = createCarViewModel.Make,
                Model = createCarViewModel.Model,
                Year = createCarViewModel.Year,
                Description = createCarViewModel.Description,
            };
            await _carApiClient.PostAsync(car);

            TempData["SuccessMessage"] = UserMessage.SuccessCarCreated + " " + car.ToString();
            return RedirectToAction("Index");
        }

        // GET: Car/Photos/5
        public async Task<IActionResult> Photos(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsNull;
                return RedirectToAction("Index");
            }

            Car? car = await _carApiClient.GetAsync((int)id);
            if (car == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCarIsNull;
                return RedirectToAction("Index");
            }

            PhotosCarViewModel photosCarViewModel = new()
            {
                Id = car.Id,
                Car = car,
                PhotoUrl1 = car.PhotoUrls.ElementAtOrDefault(0),
                PhotoUrl2 = car.PhotoUrls.ElementAtOrDefault(1),
                PhotoUrl3 = car.PhotoUrls.ElementAtOrDefault(2),
            };
            return View(photosCarViewModel);
        }

        // POST: Car/Photos/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Photos(int id,  PhotosCarViewModel photosCarViewModel)
        {
            if (id != photosCarViewModel.Id)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsInvalid;
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                TempData["WarningMessage"] = UserMessage.WarningInvalidFormData;
                return View(photosCarViewModel);
            }

            Car? car = await _carApiClient.GetAsync(photosCarViewModel.Id);
            if (car == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCarIsNull;
                return RedirectToAction("Index");
            }

            car.PhotoUrls = new List<string>();
            string photo1 = photosCarViewModel.PhotoUrl1 ?? "".Trim();
            string photo2 = photosCarViewModel.PhotoUrl2 ?? "".Trim();
            string photo3 = photosCarViewModel.PhotoUrl3 ?? "".Trim();
            if (photo1 != "")
            {
                car.PhotoUrls.Add(photo1);
            }
            if (photo2 != "")
            {
                car.PhotoUrls.Add(photo2);
            }
            if (photo3 != "")
            {
                car.PhotoUrls.Add(photo3);
            }
            await _carApiClient.PutAsync(car);

            TempData["SuccessMessage"] = UserMessage.SuccessCarPhotosUpdated + " " + car.ToString();
            return RedirectToAction("Photos");
        }

        // GET: Car/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsNull;
                return RedirectToAction("Index");
            }

            Car? car = await _carApiClient.GetAsync((int)id);
            if (car == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCarIsNull;
                return RedirectToAction("Index");
            }

            EditCarViewModel editCarViewModel = new EditCarViewModel()
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                Description = car.Description,
            };
            return View(editCarViewModel);
        }

        // POST: Car/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCarViewModel editCarViewModel)
        {
            if (id != editCarViewModel.Id)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsInvalid;
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                TempData["WarningMessage"] = UserMessage.WarningInvalidFormData;
                return View(editCarViewModel);
            }

            Car? car = await _carApiClient.GetAsync(editCarViewModel.Id);
            if (car == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCarIsNull;
                return RedirectToAction("Index");
            }

            car.Make = editCarViewModel.Make;
            car.Model = editCarViewModel.Model;
            car.Year = editCarViewModel.Year;
            car.Description = editCarViewModel.Description;
            await _carApiClient.PutAsync(car);

            TempData["SuccessMessage"] = UserMessage.SuccessCarUpdated + " " + car.ToString();
            return RedirectToAction("Index");
        }

        // GET: Car/DeleteAsync/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsNull;
                return RedirectToAction("Index");
            }

            Car? car = await _carApiClient.GetAsync((int)id);
            if (car == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCarIsNull;
                return RedirectToAction("Index");
            }

            DeleteCarViewModel deleteCarViewModel = new DeleteCarViewModel()
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                Description = car.Description,
            };
            return View(deleteCarViewModel);
        }

        // POST: Car/DeleteAsync/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Car? car = await _carApiClient.GetAsync(id);
            if (car == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCarIsNull;
                return RedirectToAction("Index");
            }

            await _carApiClient.DeleteAsync(car.Id);

            TempData["SuccessMessage"] = UserMessage.SuccessCarDeleted + " " + car.ToString();
            return RedirectToAction("Index");
        }

    }
}
