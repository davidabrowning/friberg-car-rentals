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
            IEnumerable<CarDto>? dtos = await response.Content.ReadFromJsonAsync<IEnumerable<CarDto>>();
            if (dtos == null)
                throw new InvalidDataException(UserMessage.ErrorResultIsNullfromApi);
            return dtos;
        }

        public async Task<CarDto> GetAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/cars/{id}");
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToFetchDataFromApi);
            CarDto? dto = await response.Content.ReadFromJsonAsync<CarDto>();
            if (dto == null)
                throw new InvalidDataException(UserMessage.ErrorResultIsNullfromApi);
            return dto;
        }

        public async Task<CarDto> PostAsync(CarDto dto)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/cars", dto);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToSendDataToApi);
            return dto;
        }

        public async Task<CarDto> PutAsync(CarDto dto)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/cars/{dto.Id}", dto);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToSendDataToApi);
            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/cars/{id}");
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToSendDataToApi);
        }
    }
}
