using FribergCarRentals.Core.Helpers;
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
        public async Task<ActionResult<List<ReservationDto>>> Get()
        {
            List<ReservationDto> reservationDtos = new();
            IEnumerable<Reservation> reservations = await _reservationService.GetAllAsync();
            foreach (Reservation reservation in reservations)
            {
                ReservationDto reservationDto = ReservationMapper.ToDto(reservation);
                reservationDtos.Add(reservationDto);
            }
            return Ok(reservationDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReservationDto>> Get(int id)
        {
            Reservation? reservation = await _reservationService.GetByIdAsync(id);
            if (reservation == null)
                return NotFound();
            ReservationDto reservationDto = ReservationMapper.ToDto(reservation);
            return Ok(reservationDto);
        }

        [HttpPost]
        public async Task<ActionResult<ReservationDto>> Post(ReservationDto reservationDto)
        {
            Reservation reservation = ReservationMapper.ToModel(reservationDto);
            await _reservationService.CreateAsync(reservation);
            ReservationDto updatedReservationDto = ReservationMapper.ToDto(reservation);
            return Ok(updatedReservationDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ReservationDto reservationDto)
        {
            if (id != reservationDto.Id)
                return BadRequest(UserMessage.ErrorIdsDoNotMatch);
            await _reservationService.UpdateAsync(ReservationMapper.ToModel(reservationDto));
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            Reservation? deletedReservation = await _reservationService.DeleteAsync(id);
            if (deletedReservation == null)
                return NotFound();
            return NoContent();
        }
    }
}
