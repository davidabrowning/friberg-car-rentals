using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.Facades;
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
        private readonly IApplicationFacade _applicationFacade;
        public CarsController(IApplicationFacade applicationFacade)
        {
            _applicationFacade = applicationFacade;
        }

        [HttpGet]
        public async Task<ActionResult<List<CarDto>>> Get()
        {
            IEnumerable<Car> cars = await _applicationFacade.GetAllCarsAsync();
            List<CarDto> carDtos = CarMapper.ToDtos(cars);
            return Ok(carDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CarDto>> Get(int id)
        {
            Car? car = await _applicationFacade.GetCarAsync(id);
            if (car == null)
                return NotFound();
            CarDto carDto = CarMapper.ToDto(car);
            return Ok(carDto);
        }

        [HttpPost]
        public async Task<ActionResult<CarDto>> Post(CarDto carDto)
        {
            Car car = CarMapper.ToNewModelWithoutId(carDto);
            await _applicationFacade.CreateCarAsync(car);
            CarDto resultCarDto = CarMapper.ToDto(car);
            return Ok(resultCarDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, CarDto carDto)
        {
            if (id != carDto.Id)
                return BadRequest(UserMessage.ErrorIdsDoNotMatch);
            Car? car = await _applicationFacade.GetCarAsync(id);
            if (car == null)
                return NotFound();
            CarMapper.UpdateModel(car, carDto);
            await _applicationFacade.UpdateCarAsync(car);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            Car? deletedCar = await _applicationFacade.DeleteCarAsync(id);
            if (deletedCar == null)
                return NotFound();
            return NoContent();
        }
    }
}
