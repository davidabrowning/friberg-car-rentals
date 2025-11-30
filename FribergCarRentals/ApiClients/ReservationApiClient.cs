using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.Mvc.ApiClients
{
    public class ReservationApiClient : ICRUDApiClient<ReservationDto>
    {
        private readonly HttpClient _httpClient;
        private const string _apiPath = "api/cars";
        public ReservationApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ReservationDto>> GetAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_apiPath);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToFetchDataFromApi);
            IEnumerable<ReservationDto>? dtos = await response.Content.ReadFromJsonAsync<IEnumerable<ReservationDto>>();
            if (dtos == null)
                throw new InvalidDataException(UserMessage.ErrorResultIsNullfromApi);
            return dtos;
        }

        public async Task<ReservationDto> GetAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_apiPath}/{id}");
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToFetchDataFromApi);
            ReservationDto? dto = await response.Content.ReadFromJsonAsync<ReservationDto>();
            if (dto == null)
                throw new InvalidDataException(UserMessage.ErrorResultIsNullfromApi);
            return dto;
        }

        public async Task<ReservationDto> PostAsync(ReservationDto dto)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_apiPath, dto);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToSendDataToApi);
            return dto;
        }

        public async Task<ReservationDto> PutAsync(ReservationDto dto)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"{_apiPath}/{dto.Id}", dto);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToSendDataToApi);
            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{_apiPath}/{id}");
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToSendDataToApi);
        }
    }
}
