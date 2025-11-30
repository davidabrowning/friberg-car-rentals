using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.Mvc.ApiClients
{
    public class AdminApiClient : ICRUDApiClient<AdminDto>
    {
        private readonly HttpClient _httpClient;
        public AdminApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<AdminDto>> GetAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/admins");
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToFetchDataFromApi);
            IEnumerable<AdminDto>? admins = await response.Content.ReadFromJsonAsync<IEnumerable<AdminDto>>();
            if (admins == null)
                throw new InvalidDataException(UserMessage.ErrorResultIsNullfromApi);
            return admins;
        }

        public async Task<AdminDto> GetAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/admins/{id}");
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToFetchDataFromApi);
            AdminDto? adminDto = await response.Content.ReadFromJsonAsync<AdminDto>();
            if (adminDto == null)
                throw new InvalidDataException(UserMessage.ErrorResultIsNullfromApi);
            return adminDto;
        }

        public async Task<AdminDto> PostAsync(AdminDto carDto)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/admins", carDto);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToSendDataToApi);
            return carDto;
        }

        public async Task<AdminDto> PutAsync(AdminDto adminDto)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/admins/{adminDto.Id}", adminDto);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToSendDataToApi);
            return adminDto;
        }

        public async Task DeleteAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/admins/{id}");
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToSendDataToApi);
        }
    }
}
