using FribergCarRentals.Core.Models;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.WebApi.Mappers
{
    public static class CarMapper
    {
        public static CarDto ToDto(Car car)
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

        public static Car ToModel(CarDto carDto)
        {
            Car car = new()
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

        public static List<CarDto> ToDtos(IEnumerable<Car> cars)
        {
            List<CarDto> carDtos = new();
            foreach (Car car in cars)
            {
                CarDto carDto = ToDto(car);
                carDtos.Add(carDto);
            }
            return carDtos;
        }

        public static List<Car> ToModels(IEnumerable<CarDto> carDtos)
        {
            List<Car> cars = new();
            foreach (CarDto carDto in carDtos)
            {
                Car car = ToModel(carDto);
                cars.Add(car);
            }
            return cars;
        }
    }
}
