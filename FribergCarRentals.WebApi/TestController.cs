using FribergCarRentals.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public TestController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet("cars-count")]
        public IActionResult CarsCount()
        {
            int carsCount = _applicationDbContext.Cars.Count();
            return Ok(carsCount);
        }
    }
}
