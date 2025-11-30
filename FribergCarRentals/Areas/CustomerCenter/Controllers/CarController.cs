using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.Mvc.Areas.CustomerCenter.Views.Car;
using FribergCarRentals.Mvc.Attributes;
using FribergCarRentals.WebApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Mvc.Areas.CustomerCenter.Controllers
{
    [RequireCustomer]
    [Area("CustomerCenter")]
    public class CarController : Controller
    {
        private readonly ICRUDApiClient<CarDto> _carDtoApiClient;
        public CarController(ICRUDApiClient<CarDto> carApiClient)
        {
            _carDtoApiClient = carApiClient;
        }
        public async Task<IActionResult> Index()
        {
            List<IndexCarViewModel> indexCarViewModelList = new();
            try
            {
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
                    indexCarViewModelList.Add(carIndexViewModel);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
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

            try
            {
                CarDto? carDto = await _carDtoApiClient.GetAsync((int)id);
                if (carDto == null)
                {
                    TempData["ErrorMessage"] = UserMessage.ErrorCarIsNull;
                    return RedirectToAction("Index");
                }

                DetailsCarViewModel detailsCarViewModel = new()
                {
                    Id = carDto.Id,
                    Make = carDto.Make,
                    Model = carDto.Model,
                    Year = carDto.Year,
                    Description = carDto.Description,
                    PhotoUrls = carDto.PhotoUrls.ToList(),
                };

                return View(detailsCarViewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
