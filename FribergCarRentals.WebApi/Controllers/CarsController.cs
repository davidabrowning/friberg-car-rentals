using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;
using FribergCarRentals.WebApi.Dtos;
using FribergCarRentals.WebApi.Mappers;
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
        public async Task<ActionResult<IEnumerable<CarDto>>> Get()
        {
            IEnumerable<Car> cars = await _carService.GetAllAsync();
            List<CarDto> carDtos = CarMapper.ToDtos(cars);
            return Ok(carDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CarDto?>> Get(int id)
        {
            Car? car = await _carService.GetByIdAsync(id);
            if (car == null)
            {
                return BadRequest("Car not found");
            }
            CarDto carDto = CarMapper.ToDto(car);
            return Ok(carDto);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CarDto carDto)
        {
            Car car = CarMapper.ToModel(carDto);
            await _carService.CreateAsync(car);
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CarDto?>> Put(int id, CarDto carDto)
        {
            if (id != carDto.Id)
            {
                return BadRequest("Id and car do not match.");
            }
            Car car = CarMapper.ToModel(carDto);
            await _carService.UpdateAsync(car);
            return Ok(carDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CarDto?>> Delete(int id)
        {
            Car? deletedCar = await _carService.DeleteAsync(id);
            if (deletedCar != null) {
                return BadRequest("Car not found.");
            }
            CarDto deletedCarDto = CarMapper.ToDto(deletedCar);
            return Ok(deletedCarDto);
        }
    }
}
