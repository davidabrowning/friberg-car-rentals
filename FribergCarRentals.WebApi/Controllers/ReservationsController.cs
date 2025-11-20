using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public async Task<IEnumerable<Reservation>> Get()
        {
            IEnumerable<Reservation> reservations = await _reservationService.GetAllAsync();
            return reservations;
        }

        [HttpGet("{id:int}")]
        public async Task<Reservation?> Get(int id)
        {
            Reservation? reservation = await _reservationService.GetByIdAsync(id);
            return reservation;
        }

        [HttpPost]
        public async Task<Reservation> Post(Reservation reservation)
        {
            await _reservationService.CreateAsync(reservation);
            return reservation;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return BadRequest("Id and reservation do not match.");
            }
            await _reservationService.UpdateAsync(reservation);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _reservationService.DeleteAsync(id);
            return NoContent();
        }
    }
}
