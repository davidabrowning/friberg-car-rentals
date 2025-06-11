using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.Data;
using FribergCarRentals.Models;
using FribergCarRentals.ViewModels;

namespace FribergCarRentals.Controllers
{
    public class CarListController : Controller
    {
        private readonly IRepository<Car> _carRepository;

        public CarListController(IRepository<Car> carRepository)
        {
            _carRepository = carRepository;
        }

        // GET: CarList
        public async Task<IActionResult> Index()
        {
            CarListViewModel carListViewModel  = new CarListViewModel();
            IEnumerable<Car> cars = await _carRepository.GetAll();
            foreach (Car car in cars)
            {
                carListViewModel.CarIds.Add(car.Id);
            }
            return View(carListViewModel);
        }

        //// GET: CarList/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var carViewModel = await _carRepository.CarIndexViewModel
        //        .FirstOrDefaultAsync(m => m.CustomerId == id);
        //    if (carViewModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(carViewModel);
        //}

        //// GET: CarList/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: CarList/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("CustomerId,Make,Model,Year,Description")] CarIndexViewModel carViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _carRepository.Add(carViewModel);
        //        await _carRepository.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(carViewModel);
        //}

        //// GET: CarList/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var carViewModel = await _carRepository.CarIndexViewModel.FindAsync(id);
        //    if (carViewModel == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(carViewModel);
        //}

        //// POST: CarList/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("CustomerId,Make,Model,Year,Description")] CarIndexViewModel carViewModel)
        //{
        //    if (id != carViewModel.CustomerId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _carRepository.Update(carViewModel);
        //            await _carRepository.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CarViewModelExists(carViewModel.CustomerId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(carViewModel);
        //}

        //// GET: CarList/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var carViewModel = await _carRepository.CarIndexViewModel
        //        .FirstOrDefaultAsync(m => m.CustomerId == id);
        //    if (carViewModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(carViewModel);
        //}

        //// POST: CarList/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var carViewModel = await _carRepository.CarIndexViewModel.FindAsync(id);
        //    if (carViewModel != null)
        //    {
        //        _carRepository.CarIndexViewModel.Delete(carViewModel);
        //    }

        //    await _carRepository.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool CarViewModelExists(int id)
        //{
        //    return _carRepository.CarIndexViewModel.Any(e => e.CustomerId == id);
        //}
    }
}
