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
                throw new InvalidOperationException(UserMessage.ErrorUnableToFetchData);
            return await response.Content.ReadFromJsonAsync<IEnumerable<AdminDto>>();
        }

        public async Task<AdminDto> GetAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/admins/{id}");
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToFetchData);
            return await response.Content.ReadFromJsonAsync<AdminDto>();
        }

        public async Task<AdminDto> PostAsync(AdminDto adminDto)
        {
            await _httpClient.PostAsJsonAsync<AdminDto>("api/admins", adminDto);
            return adminDto;
        }

        public async Task<AdminDto> PutAsync(AdminDto adminDto)
        {
            await _httpClient.PutAsJsonAsync<AdminDto>($"api/admins/{adminDto.Id}", adminDto);
            return adminDto;
        }
        public async Task DeleteAsync(int id)
        {
            await _httpClient.DeleteAsync($"api/admins/{id}");
        }
    }
}
