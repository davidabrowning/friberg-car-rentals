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
                return NotFound();
            }

            Car? car = await _carService.GetByIdAsync((int)id);
            if (car == null)
            {
                return NotFound();
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
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(editCarViewModel);
            }

            Car car = await _carService.GetByIdAsync(editCarViewModel.Id);
            if (car == null)
            {
                return NotFound();
            }

            car.Make = editCarViewModel.Make;
            car.Model = editCarViewModel.Model;
            car.Year = editCarViewModel.Year;
            car.Description = editCarViewModel.Description;
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
            return RedirectToAction(nameof(Index));
        }

    }
}
