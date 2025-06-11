using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.Data;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Authorization;
using FribergCarRentals.Areas.Administration.Helpers;
using FribergCarRentals.Areas.Administration.ViewModels;

namespace FribergCarRentals.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administration")]
    public class CarController : Controller
    {
        private readonly IRepository<Car> _carRepository;

        public CarController(IRepository<Car> carRepository)
        {
            _carRepository = carRepository;
        }

        // GET: Car
        public async Task<IActionResult> Index()
        {
            List<CarIndexViewModel> carIndexViewModels = new();
            IEnumerable<Car> cars = await _carRepository.GetAllAsync();
            foreach (Car car in cars)
            {
                CarIndexViewModel carIndexViewModel = ViewModelMakerHelper.MakeCarIndexViewModel(car);
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

            Car? car = await _carRepository.GetByIdAsync((int)id);
            if (car == null)
            {
                return NotFound();
            }

            CarIndexViewModel carIndexViewModel = ViewModelMakerHelper.MakeCarIndexViewModel(car);
            return View(carIndexViewModel);
        }

        // GET: Car/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Car/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarCreateViewModel carCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(carCreateViewModel);
            }

            Car car = ViewModelToCreateHelper.CreateNewCar(carCreateViewModel);
            await _carRepository.AddAsync(car);
            return RedirectToAction(nameof(Index));
        }

        // GET: Car/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Car? car = await _carRepository.GetByIdAsync((int)id);
            if (car == null)
            {
                return NotFound();
            }

            CarEditViewModel carEditViewModel = ViewModelMakerHelper.MakeCarEditViewModel(car);
            return View(carEditViewModel);
        }

        // POST: Car/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

            try
            {
                Car car = await _carRepository.GetByIdAsync(carEditViewModel.Id);
                ViewModelToUpdateHelper.UpdateExistingCar(car, carEditViewModel);
                await _carRepository.UpdateAsync(car);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CarExists(carEditViewModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Car/DeleteAsync/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Car? car = await _carRepository.GetByIdAsync((int)id);
            if (car == null)
            {
                return NotFound();
            }

            CarEditViewModel carEditViewModel = ViewModelMakerHelper.MakeCarEditViewModel(car);
            return View(carEditViewModel);
        }

        // POST: Car/DeleteAsync/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Car? car = await _carRepository.GetByIdAsync(id);
            if (car != null)
            {
                await _carRepository.DeleteAsync(car.Id);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CarExists(int id)
        {
            return await _carRepository.IdExistsAsync(id);
        }
    }
}
