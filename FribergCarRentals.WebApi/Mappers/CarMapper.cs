using FribergCarRentals.Core.Models;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.WebApi.Mappers
{
    public static class CarMapper
    {
        public static Dtos.CarDto ToDto(Core.Models.Car car)
        {
            Dtos.CarDto carDto = new() { 
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                Description = car.Description,
                PhotoUrls = car.PhotoUrls
            };
            return carDto;
        }

        public static Core.Models.Car ToModel(Dtos.CarDto carDto)
        {
            Core.Models.Car car = new()
            {
                Id = carDto.Id,
                Make = carDto.Make,
                Model = carDto.Model,
                Year = carDto.Year,
                Description = carDto.Description,
                PhotoUrls = carDto.PhotoUrls
            };
            return car;
        }
    }
}
