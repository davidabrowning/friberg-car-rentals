using FribergCarRentals.Areas.CustomerCenter.Views.Car;
using FribergCarRentals.Helpers;
using FribergCarRentals.Core.Interfaces;
using FribergCarRentals.Core.Models;
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
            List<IndexCarViewModel> indexCarViewModelList = new();
            foreach (Car car in await _carService.GetAllAsync())
            {
                IndexCarViewModel carIndexViewModel = new()
                {
                    Id = car.Id,
                    Make = car.Make,
                    Model = car.Model,
                    Year = car.Year,
                    Description = car.Description,
                    PhotoUrl = car.PhotoUrls.ElementAtOrDefault(0) ?? string.Empty,
                };
                indexCarViewModelList.Add(carIndexViewModel);
            }
            return View(indexCarViewModelList);
        }

        // GET: CustomerCenter/Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsNull;
                return RedirectToAction("Index");
            }

            Car? car = await _carService.GetByIdAsync((int)id);
            if (car == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorCarIsNull;
                return RedirectToAction("Index");
            }

            DetailsCarViewModel detailsCarViewModel = new()
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                Description = car.Description,
                PhotoUrls = car.PhotoUrls,
            };

            return View(detailsCarViewModel);
        }
    }
}
