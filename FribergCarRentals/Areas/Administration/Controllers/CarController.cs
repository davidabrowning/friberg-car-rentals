using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Authorization;
using FribergCarRentals.Areas.Administration.ViewModels;
using FribergCarRentals.Interfaces;

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
            List<CarIndexViewModel> carIndexViewModels = new();
            IEnumerable<Car> cars = await _carService.GetAllAsync();
            foreach (Car car in cars)
            {
                CarIndexViewModel carIndexViewModel = new CarIndexViewModel()
                {
                    Id = car.Id,
                    Make = car.Make,
                    Model = car.Model,
                    Year = car.Year,
                    Description = car.Description,
                    ReservationIds = car.Reservations.Select(r => r.Id).ToList(),
                };
                carIndexViewModels.Add(carIndexViewModel);
            }
            return View(carIndexViewModels);
        }

        // GET: Car/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Car? car = await _carService.GetByIdAsync((int)id);
            if (car == null)
            {
                return NotFound();
            }

            CarIndexViewModel carIndexViewModel = new CarIndexViewModel()
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                Description = car.Description,
                ReservationIds = car.Reservations.Select(r => r.Id).ToList(),
            };
            return View(carIndexViewModel);
        }

        // GET: Car/Create
        public IActionResult Create()
        {
            CarCreateViewModel carCerateViewModel = new();
            return View(carCerateViewModel);
        }

        // POST: Car/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarCreateViewModel carCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(carCreateViewModel);
            }

            Car car = new()
            {
                Make = carCreateViewModel.Make,
                Model = carCreateViewModel.Model,
                Year = carCreateViewModel.Year,
                Description = carCreateViewModel.Description,
            };
            await _carService.CreateAsync(car);

            return RedirectToAction(nameof(Index));
        }

        // GET: Car/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Car? car = await _carService.GetByIdAsync((int)id);
            if (car == null)
            {
                return NotFound();
            }

            CarEditViewModel carEditViewModel = new CarEditViewModel()
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                Description = car.Description,
            };
            return View(carEditViewModel);
        }

        // POST: Car/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CarEditViewModel carEditViewModel)
        {
            if (id != carEditViewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(carEditViewModel);
            }

            Car car = await _carService.GetByIdAsync(carEditViewModel.Id);
            if (car == null)
            {
                return NotFound();
            }

            car.Make = carEditViewModel.Make;
            car.Model = carEditViewModel.Model;
            car.Year = carEditViewModel.Year;
            car.Description = carEditViewModel.Description;
            await _carService.UpdateAsync(car);

            return RedirectToAction(nameof(Index));
        }

        // GET: Car/DeleteAsync/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Car? car = await _carService.GetByIdAsync((int)id);
            if (car == null)
            {
                return NotFound();
            }

            CarEditViewModel carEditViewModel = new CarEditViewModel()
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                Description = car.Description,
            };
            return View(carEditViewModel);
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
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CarExists(int id)
        {
            return await _carService.IdExistsAsync(id);
        }
    }
}
