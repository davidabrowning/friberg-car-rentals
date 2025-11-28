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
    public class ReservationsController : ControllerBase
    {
        IApplicationFacade _applicationFacade;
        public ReservationsController(IApplicationFacade applicationFacade)
        {
            _applicationFacade = applicationFacade;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> Get()
        {
            List<ReservationDto> reservationDtos = new();
            IEnumerable<Reservation> reservations = await _applicationFacade.GetAllReservationsAsync();
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
            Reservation? reservation = await _applicationFacade.GetReservationAsync(id);
            if (reservation == null)
                return NotFound();
            ReservationDto reservationDto = ReservationMapper.ToDto(reservation);
            return Ok(reservationDto);
        }

        [HttpPost]
        public async Task<ActionResult<ReservationDto>> Post(ReservationDto reservationDto)
        {
            Car? car = await _applicationFacade.GetCarAsync(reservationDto.CarDto.Id);
            if (car == null)
                return BadRequest(UserMessage.ErrorCarIsNull);
            Customer? customer = await _applicationFacade.GetCustomerAsync(reservationDto.CustomerDto.Id);
            if (customer == null)
                return BadRequest(UserMessage.ErrorCustomerIsNull);

            Reservation reservation = ReservationMapper.ToNewModelWithoutId(reservationDto, car, customer);
            await _applicationFacade.CreateReservationAsync(reservation);
            ReservationDto updatedReservationDto = ReservationMapper.ToDto(reservation);
            return Ok(updatedReservationDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ReservationDto reservationDto)
        {
            if (id != reservationDto.Id)
                return BadRequest(UserMessage.ErrorIdsDoNotMatch);
            Car? car = await _applicationFacade.GetCarAsync(reservationDto.CarDto.Id);
            if (car == null)
                return BadRequest(UserMessage.ErrorCarIsNull);
            Customer? customer = await _applicationFacade.GetCustomerAsync(reservationDto.CustomerDto.Id);
            if (customer == null)
                return BadRequest(UserMessage.ErrorCustomerIsNull);

            Reservation reservation = ReservationMapper.ToNewModelWithoutId(reservationDto, car, customer);
            await _applicationFacade.UpdateReservationAsync(reservation);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            Reservation? deletedReservation = await _applicationFacade.DeleteReservationAsync(id);
            if (deletedReservation == null)
                return NotFound();
            return NoContent();
        }
    }
}
