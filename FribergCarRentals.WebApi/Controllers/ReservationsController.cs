using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;
using FribergCarRentals.WebApi.Dtos;
using FribergCarRentals.WebApi.Mappers;
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
        public async Task<IEnumerable<ReservationDto>> Get()
        {
            List<ReservationDto> reservationDtos = new();
            IEnumerable<Reservation> reservations = await _reservationService.GetAllAsync();
            foreach (Reservation reservation in reservations)
            {
                ReservationDto reservationDto = ReservationMapper.ToDto(reservation);
                reservationDtos.Add(reservationDto);
            }
            return reservationDtos;
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
