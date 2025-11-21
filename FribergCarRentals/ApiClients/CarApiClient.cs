using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.Mvc.ApiClients
{
    public class CarApiClient : IApiClient<CarDto>
    {
        private readonly HttpClient _httpClient;
        public CarApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CarDto>> GetAsync()
        {
            List<CarDto> carDtoList = await _httpClient.GetFromJsonAsync<List<CarDto>>("api/cars") ?? new();
            return carDtoList;
        }

        public async Task<CarDto?> GetAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<CarDto?>($"api/cars/{id}");
        }

        public async Task<CarDto> PostAsync(CarDto carDto)
        {
            await _httpClient.PostAsJsonAsync<CarDto>("api/cars", carDto);
            return carDto;
        }

        public async Task PutAsync(CarDto carDto)
        {
            await _httpClient.PutAsJsonAsync<CarDto>($"api/cars/{carDto.Id}", carDto);
        }

        public async Task DeleteAsync(int id)
        {
            CarDto? deletedCarDto = await _httpClient.DeleteFromJsonAsync<CarDto?>($"api/cars/{id}");
        }
    }
}
