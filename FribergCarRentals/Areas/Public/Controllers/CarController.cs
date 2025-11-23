using FribergCarRentals.Mvc.Areas.Public.Views.Car;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Mvc.Areas.Public.Controllers
{
    [Area("Public")]
    public class CarController : Controller
    {
        private readonly ICRUDApiClient<CarDto> _carDtoApiClient;
        public CarController(ICRUDApiClient<CarDto> carDtoApiClient)
        {
            _carDtoApiClient = carDtoApiClient;
        }
        public async Task<IActionResult> Index()
        {
            List<IndexCarViewModel> carIndexViewModelList = new();
            foreach (CarDto carDto in await _carDtoApiClient.GetAsync())
            {
                IndexCarViewModel carIndexViewModel = new()
                {
                    Id = carDto.Id,
                    Make = carDto.Make,
                    Model = carDto.Model,
                    Year = carDto.Year,
                    Description = carDto.Description,
                    PhotoUrl = carDto.PhotoUrls.ElementAtOrDefault(0) ?? string.Empty,
                };
                carIndexViewModelList.Add(carIndexViewModel);
            }
            return View(carIndexViewModelList);
        }
    }
}
