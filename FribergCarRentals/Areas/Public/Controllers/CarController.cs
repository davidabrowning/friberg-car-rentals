using FribergCarRentals.Areas.Public.Views.Car;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Areas.Public.Controllers
{
    [Area("Public")]
    public class CarController : Controller
    {
        private readonly ICarApiClient _carApiClient;
        public CarController(ICarApiClient carApiClient)
        {
            _carApiClient = carApiClient;
        }
        public async Task<IActionResult> Index()
        {
            List<IndexCarViewModel> carIndexViewModelList = new();
            foreach (Car car in await _carApiClient.GetAllAsync())
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
                carIndexViewModelList.Add(carIndexViewModel);
            }
            return View(carIndexViewModelList);
        }
    }
}
