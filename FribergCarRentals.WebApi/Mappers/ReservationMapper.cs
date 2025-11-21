using FribergCarRentals.Core.Models;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.WebApi.Mappers
{
    public static class ReservationMapper
    {
        public static ReservationDto ToDto(Reservation reservation)
        {
            ReservationDto dto = new()
            {
                Id = reservation.Id,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                CarDto = CarMapper.ToDto(reservation.Car),
                CustomerDto = CustomerMapper.ToDto(reservation.Customer),
            };
            return dto;
        }
        public static Reservation ToModel(ReservationDto reservationDto)
        {
            Reservation model = new()
            {
                Id = reservationDto.Id,
                StartDate = reservationDto.StartDate,
                EndDate = reservationDto.EndDate,
                Car = CarMapper.ToModel(reservationDto.CarDto),
                Customer = CustomerMapper.ToModel(reservationDto.CustomerDto)
            };
            return model;
        }
    }
}
