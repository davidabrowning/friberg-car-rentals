using FribergCarRentals.Areas.Public.ViewModels;
using FribergCarRentals.Interfaces;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Areas.Public.Controllers
{
    [Area("Public")]
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
    }
}
