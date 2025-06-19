using FribergCarRentals.Areas.CustomerCenter.ViewModels;
using FribergCarRentals.Interfaces;
using FribergCarRentals.Models;
using FribergCarRentals.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Areas.CustomerCenter.Controllers
{
    [Authorize(Roles = "Customer")]
    [Area("CustomerCenter")]
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        public CarController(ICarService carService)
        {
            _carService = carService;
        }
        public async Task<IActionResult> Index()
        {
            List<CarIndexViewModel> carIndexViewModelList = new();
            foreach (Car car in await _carService.GetAllAsync())
            {
                CarIndexViewModel carIndexViewModel = new()
                {
                    Id = car.Id,
                    Make = car.Make,
                    Model = car.Model,
                    Year = car.Year,
                    Description = car.Description,
                };
                carIndexViewModelList.Add(carIndexViewModel);
            }
            return View(carIndexViewModelList);
        }

        // GET: CustomerCenter/Customer/Details/5
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

            CarDetailsViewModel carDetailViewModel = new()
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                Description = car.Description,
            };

            return View(carDetailViewModel);
        }
    }
}
