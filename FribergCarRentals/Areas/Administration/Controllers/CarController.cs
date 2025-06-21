using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Authorization;
using FribergCarRentals.Interfaces;
using FribergCarRentals.Areas.Administration.Views.Car;

namespace FribergCarRentals.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administration")]
    public class CarController : Controller
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        // GET: Car
        public async Task<IActionResult> Index()
        {
            List<IndexCarViewModel> indexCarViewModelList = new();
            IEnumerable<Car> cars = await _carService.GetAllAsync();
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
                TempData["ErrorMessage"] = "Invalid car id number format.";
                return RedirectToAction("Index");
            }

            Car? car = await _carService.GetByIdAsync((int)id);
            if (car == null)
            {
                TempData["ErrorMessage"] = "Unable to find car with that id.";
                return RedirectToAction("Index");
            }

            DetailsCarViewModel detailsCarViewModel = new DetailsCarViewModel()
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                Description = car.Description,
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
            await _carService.CreateAsync(car);

            TempData["SuccessMessage"] = $"New car created: {car.ToString()}.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Car/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Invalid car id number format.";
                return RedirectToAction("Index");
            }

            Car? car = await _carService.GetByIdAsync((int)id);
            if (car == null)
            {
                TempData["ErrorMessage"] = "Unable to find car with that id.";
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
                TempData["ErrorMessage"] = "Invalid car id.";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                TempData["WarningMessage"] = "Invalid form information.";
                return View(editCarViewModel);
            }

            Car car = await _carService.GetByIdAsync(editCarViewModel.Id);
            if (car == null)
            {
                TempData["ErrorMessage"] = "Unable to find car with that id.";
                return RedirectToAction("Index");
            }

            car.Make = editCarViewModel.Make;
            car.Model = editCarViewModel.Model;
            car.Year = editCarViewModel.Year;
            car.Description = editCarViewModel.Description;
            await _carService.UpdateAsync(car);

            TempData["SuccessMessage"] = $"Car information successfully updated: {car.ToString()}";
            return RedirectToAction(nameof(Index));
        }

        // GET: Car/DeleteAsync/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Invalid car id.";
                return RedirectToAction(nameof(Index));
            }

            Car? car = await _carService.GetByIdAsync((int)id);
            if (car == null)
            {
                TempData["ErrorMessage"] = "Unable to find car with that id.";
                return RedirectToAction(nameof(Index));
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
            Car? car = await _carService.GetByIdAsync(id);
            if (car != null)
            {
                await _carService.DeleteAsync(car.Id);
            }

            TempData["SuccessMessage"] = $"Car successfully deleted: {car.ToString()}";
            return RedirectToAction(nameof(Index));
        }

    }
}
