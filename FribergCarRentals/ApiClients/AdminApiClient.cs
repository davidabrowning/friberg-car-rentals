using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.Mvc.ApiClients
{
    public class AdminApiClient : IApiClient<AdminDto>
    {
        private readonly HttpClient _httpClient;
        public AdminApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<AdminDto>> GetAsync()
        {
            List<AdminDto> adminDtoList = await _httpClient.GetFromJsonAsync<List<AdminDto>>("api/admins") ?? new();
            return adminDtoList;
        }

        public async Task<AdminDto?> GetAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<AdminDto?>($"api/admins/{id}");
        }

        public async Task<AdminDto> PostAsync(AdminDto adminDto)
        {
            await _httpClient.PostAsJsonAsync<AdminDto>("api/admins", adminDto);
            return adminDto;
        }

        public async Task PutAsync(AdminDto adminDto)
        {
            await _httpClient.PutAsJsonAsync<AdminDto>($"api/admins/{adminDto.Id}", adminDto);
        }
        public async Task DeleteAsync(int id)
        {
            AdminDto? deletedAdminDto = await _httpClient.DeleteFromJsonAsync<AdminDto?>($"api/admins/{id}");
        }
    }
}
