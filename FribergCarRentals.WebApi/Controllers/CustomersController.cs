using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.WebApi.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IUserService _userService;
        public CustomersController(IUserService userService)
        {
            _userService = userService;
        }
    }
}
