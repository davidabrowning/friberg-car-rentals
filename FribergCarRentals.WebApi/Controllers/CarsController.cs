using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;
using FribergCarRentals.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

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
    }
}
