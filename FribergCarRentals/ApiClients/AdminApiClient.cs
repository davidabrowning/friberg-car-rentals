using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.Mvc.ApiClients
{
    public class AdminApiClient : ICRUDApiClient<AdminDto>
    {
        private readonly HttpClient _httpClient;
        private const string _apiPath = "api/cars";

        public AdminApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<AdminDto>> GetAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_apiPath);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToFetchDataFromApi);
            IEnumerable<AdminDto>? dtos = await response.Content.ReadFromJsonAsync<IEnumerable<AdminDto>>();
            if (dtos == null)
                throw new InvalidDataException(UserMessage.ErrorResultIsNullfromApi);
            return dtos;
        }

        public async Task<AdminDto> GetAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_apiPath}/{id}");
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToFetchDataFromApi);
            AdminDto? dto = await response.Content.ReadFromJsonAsync<AdminDto>();
            if (dto == null)
                throw new InvalidDataException(UserMessage.ErrorResultIsNullfromApi);
            return dto;
        }

        public async Task<AdminDto> PostAsync(AdminDto dto)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_apiPath, dto);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToSendDataToApi);
            return dto;
        }

        public async Task<AdminDto> PutAsync(AdminDto dto)
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
