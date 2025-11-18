using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;
        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetAll()
        {
            var cars = await _carService.GetAllAsync();
            return Ok(cars);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Car?>> GetById(int id)
        {
            Car? car = await _carService.GetByIdAsync(id);
            return Ok(car);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(Car car)
        {
            await _carService.CreateAsync(car);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Car?>> DeleteAsync(int id)
        {
            Car? deletedCar = await _carService.DeleteAsync(id);
            return Ok(deletedCar);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Car?>> PutAsync(int id, Car car)
        {
            if (id != car.Id)
            {
                return BadRequest("Id and car do not match.");
            }
            await _carService.UpdateAsync(car);
            return Ok(car);
        }
    }
}
