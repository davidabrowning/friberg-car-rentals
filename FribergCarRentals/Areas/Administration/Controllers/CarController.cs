using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.Mvc.Areas.Administration.Views.Car;
using FribergCarRentals.Mvc.Attributes;
using FribergCarRentals.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Mvc.Areas.Administration.Controllers
{
    [RequireAdmin]
    [Area("Administration")]
    public class CarController : Controller
    {
        private readonly ICRUDApiClient<CarDto> _carDtoApiClient;

        public CarController(ICRUDApiClient<CarDto> carDtoClient)
        {
            _carDtoApiClient = carDtoClient;
        }

        // GET: Car
        public async Task<IActionResult> Index()
        {
            List<IndexCarViewModel> indexCarViewModelList = new();
            try
            {
                IEnumerable<CarDto> carDtos = await _carDtoApiClient.GetAsync();
                foreach (CarDto carDto in carDtos)
                {
                    IndexCarViewModel indexCarViewModel = new IndexCarViewModel()
                    {
                        Id = carDto.Id,
                        Make = carDto.Make,
                        Model = carDto.Model,
                        Year = carDto.Year,
                        Description = carDto.Description,
                        // ReservationIds = carDto.Reservations.Select(r => r.Id).ToList(),
                    };
                    indexCarViewModelList.Add(indexCarViewModel);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
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

            try
            {
                CarDto? carDto = await _carDtoApiClient.GetAsync((int)id);
                if (carDto == null)
                {
                    TempData["ErrorMessage"] = UserMessage.ErrorCarIsNull;
                    return RedirectToAction("Index");
                }

                DetailsCarViewModel detailsCarViewModel = new DetailsCarViewModel()
                {
                    Id = carDto.Id,
                    Make = carDto.Make,
                    Model = carDto.Model,
                    Year = carDto.Year,
                    Description = carDto.Description,
                    PhotoUrls = carDto.PhotoUrls.ToList(),
                    // ReservationIds = carDto.Reservations.Select(r => r.Id).ToList(),
                };
                return View(detailsCarViewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
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

            try
            {
                CarDto carDto = new()
                {
                    Id = 0,
                    Make = createCarViewModel.Make,
                    Model = createCarViewModel.Model,
                    Year = createCarViewModel.Year,
                    Description = createCarViewModel.Description,
                    PhotoUrls = new List<string>()
                };
                await _carDtoApiClient.PostAsync(carDto);

                TempData["SuccessMessage"] = UserMessage.SuccessCarCreated + " " + carDto.ToString();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Car/Photos/5
        public async Task<IActionResult> Photos(int? id)
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

                PhotosCarViewModel photosCarViewModel = new()
                {
                    Id = carDto.Id,
                    CarDto = carDto,
                    PhotoUrl1 = carDto.PhotoUrls.ElementAtOrDefault(0),
                    PhotoUrl2 = carDto.PhotoUrls.ElementAtOrDefault(1),
                    PhotoUrl3 = carDto.PhotoUrls.ElementAtOrDefault(2),
                };
                return View(photosCarViewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
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

            try
            {
                CarDto? carDto = await _carDtoApiClient.GetAsync(photosCarViewModel.Id);
                if (carDto == null)
                {
                    TempData["ErrorMessage"] = UserMessage.ErrorCarIsNull;
                    return RedirectToAction("Index");
                }

                List<string> photoUrls = new();
                string photo1 = photosCarViewModel.PhotoUrl1 ?? "".Trim();
                string photo2 = photosCarViewModel.PhotoUrl2 ?? "".Trim();
                string photo3 = photosCarViewModel.PhotoUrl3 ?? "".Trim();
                if (photo1 != "")
                {
                    photoUrls.Add(photo1);
                }
                if (photo2 != "")
                {
                    photoUrls.Add(photo2);
                }
                if (photo3 != "")
                {
                    photoUrls.Add(photo3);
                }
                carDto.PhotoUrls = photoUrls;
                await _carDtoApiClient.PutAsync(carDto);

                TempData["SuccessMessage"] = UserMessage.SuccessCarPhotosUpdated + " " + carDto.ToString();
                return RedirectToAction("Photos");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Car/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

                EditCarViewModel editCarViewModel = new EditCarViewModel()
                {
                    Id = carDto.Id,
                    Make = carDto.Make,
                    Model = carDto.Model,
                    Year = carDto.Year,
                    Description = carDto.Description,
                };
                return View(editCarViewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
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

            try
            {
                CarDto? carDto = await _carDtoApiClient.GetAsync(editCarViewModel.Id);
                if (carDto == null)
                {
                    TempData["ErrorMessage"] = UserMessage.ErrorCarIsNull;
                    return RedirectToAction("Index");
                }

                carDto.Make = editCarViewModel.Make;
                carDto.Model = editCarViewModel.Model;
                carDto.Year = editCarViewModel.Year;
                carDto.Description = editCarViewModel.Description;
                await _carDtoApiClient.PutAsync(carDto);

                TempData["SuccessMessage"] = UserMessage.SuccessCarUpdated + " " + carDto.ToString();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Car/DeleteAsync/5
        public async Task<IActionResult> Delete(int? id)
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

                DeleteCarViewModel deleteCarViewModel = new DeleteCarViewModel()
                {
                    Id = carDto.Id,
                    Make = carDto.Make,
                    Model = carDto.Model,
                    Year = carDto.Year,
                    Description = carDto.Description,
                };
                return View(deleteCarViewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Car/DeleteAsync/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                CarDto? carDto = await _carDtoApiClient.GetAsync(id);
                if (carDto == null)
                {
                    TempData["ErrorMessage"] = UserMessage.ErrorCarIsNull;
                    return RedirectToAction("Index");
                }

                await _carDtoApiClient.DeleteAsync(carDto.Id);

                TempData["SuccessMessage"] = UserMessage.SuccessCarDeleted + " " + carDto.ToString();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
