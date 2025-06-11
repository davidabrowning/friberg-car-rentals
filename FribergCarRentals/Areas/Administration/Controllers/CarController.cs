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
using FribergCarRentals.ViewModels;
using FribergCarRentals.Areas.Administration.Helpers;

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
            List<CarViewModel> carViewModels = new();
            IEnumerable<Car> cars = await _carRepository.GetAll();
            foreach (Car car in cars)
            {
                CarViewModel carViewModel = ViewModelMappingHelper.GetCarViewModel(car);
                carViewModels.Add(carViewModel);
            }
            return View(carViewModels);
        }

        // GET: Car/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Car? car = await _carRepository.GetById((int)id);
            if (car == null)
            {
                return NotFound();
            }

            CarViewModel carViewModel = ViewModelMappingHelper.GetCarViewModel(car);
            return View(carViewModel);
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
        public async Task<IActionResult> Create([Bind("Id,Make,Model,Year,Description")] CarViewModel carViewModel)
        {
            if (ModelState.IsValid)
            {
                List<Reservation> reservations = null;
                Car car = ViewModelMappingHelper.CreateNewCar(carViewModel, reservations);
                await _carRepository.Add(car);
                return RedirectToAction(nameof(Index));
            }
            return View(carViewModel);
        }

        // GET: Car/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Car? car = await _carRepository.GetById((int)id);
            if (car == null)
            {
                return NotFound();
            }

            CarViewModel carViewModel = ViewModelMappingHelper.GetCarViewModel(car);
            return View(carViewModel);
        }

        // POST: Car/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Make,Model,Year,Description")] CarViewModel carViewModel)
        {
            if (id != carViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    List<Reservation> reservations = null;
                    Car car = ViewModelMappingHelper.CreateNewCar(carViewModel, reservations);
                    await _carRepository.Update(car);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CarExists(carViewModel.Id))
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
            return View(carViewModel);
        }

        // GET: Car/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Car? car = await _carRepository.GetById((int)id);
            if (car == null)
            {
                return NotFound();
            }

            CarViewModel carViewModel = ViewModelMappingHelper.GetCarViewModel(car);
            return View(carViewModel);
        }

        // POST: Car/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Car? car = await _carRepository.GetById(id);
            if (car != null)
            {
                await _carRepository.Delete(car.Id);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CarExists(int id)
        {
            return await _carRepository.IdExists(id);
        }
    }
}
