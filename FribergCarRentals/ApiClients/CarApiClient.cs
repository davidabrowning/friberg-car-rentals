using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.Mvc.ApiClients
{
    public class CarApiClient : ICRUDApiClient<CarDto>
    {
        private readonly HttpClient _httpClient;
        public CarApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CarDto>> GetAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/cars");
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToFetchDataFromApi);
            IEnumerable<CarDto>? cars = await response.Content.ReadFromJsonAsync<IEnumerable<CarDto>>();
            if (cars == null)
                throw new InvalidDataException(UserMessage.ErrorResultIsNullfromApi);
            return cars;
        }

        public async Task<CarDto> GetAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/cars/{id}");
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToFetchDataFromApi);
            CarDto? carDto = await response.Content.ReadFromJsonAsync<CarDto>();
            if (carDto == null)
                throw new InvalidDataException(UserMessage.ErrorResultIsNullfromApi);
            return carDto;
        }

        public async Task<CarDto> PostAsync(CarDto carDto)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/cars", carDto);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToSendDataToApi);
            return carDto;
        }

        public async Task<CarDto> PutAsync(CarDto carDto)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/cars/{carDto.Id}", carDto);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToSendDataToApi);
            return carDto;
        }

        public async Task DeleteAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/cars/{id}");
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToSendDataToApi);
        }
    }
}
