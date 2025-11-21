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
        public async Task<ReservationDto?> Get(int id)
        {
            Reservation? reservation = await _reservationService.GetByIdAsync(id);
            if (reservation == null)
            {
                return null;
            }
            return ReservationMapper.ToDto(reservation);
        }

        [HttpPost]
        public async Task<ReservationDto> Post(ReservationDto reservationDto)
        {
            await _reservationService.CreateAsync(ReservationMapper.ToModel(reservationDto));
            return reservationDto;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ReservationDto reservationDto)
        {
            if (id != reservationDto.Id)
            {
                return BadRequest("Id and reservation do not match.");
            }
            await _reservationService.UpdateAsync(ReservationMapper.ToModel(reservationDto));
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
