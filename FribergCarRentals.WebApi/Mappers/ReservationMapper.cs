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
        public static Reservation ToNewModelWIthoutId(ReservationDto reservationDto, Car car, Customer customer)
        {
            Reservation model = new()
            {
                StartDate = reservationDto.StartDate,
                EndDate = reservationDto.EndDate,
                Car = car,
                Customer = customer,
            };
            return model;
        }

        public static void UpdateModel(Reservation reservation, ReservationDto reservationDto)
        {
            reservation.StartDate = reservationDto.StartDate;
            reservation.EndDate = reservationDto.EndDate;   
        }
    }
}
