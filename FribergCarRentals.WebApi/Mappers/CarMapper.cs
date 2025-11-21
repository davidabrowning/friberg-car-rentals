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

        public static List<Dtos.CarDto> ToDtos(IEnumerable<Core.Models.Car> cars)
        {
            List<Dtos.CarDto> carDtos = new();
            foreach (Core.Models.Car car in cars)
            {
                Dtos.CarDto carDto = ToDto(car);
                carDtos.Add(carDto);
            }
            return carDtos;
        }

        public static List<Core.Models.Car> ToModels(IEnumerable<Dtos.CarDto> carDtos)
        {
            List<Core.Models.Car> cars = new();
            foreach (Dtos.CarDto carDto in carDtos)
            {
                Core.Models.Car car = ToModel(carDto);
                cars.Add(car);
            }
            return cars;
        }
    }
}
