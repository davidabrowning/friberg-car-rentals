using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.Mvc.ApiClients
{
    public class CustomerApiClient : IApiClient<CustomerDto>
    {
        private readonly HttpClient _httpClient;
        public CustomerApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CustomerDto>> GetAsync()
        {
            List<CustomerDto> customerDtoList = await _httpClient.GetFromJsonAsync<List<CustomerDto>>("api/customers") ?? new();
            return customerDtoList;
        }

        public async Task<CustomerDto?> GetAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<CustomerDto?>($"api/customers/{id}");
        }

        public async Task<CustomerDto> PostAsync(CustomerDto customerDto)
        {
            await _httpClient.PostAsJsonAsync<CustomerDto>("api/customers", customerDto);
            return customerDto;
        }

        public async Task<CustomerDto> PutAsync(CustomerDto adminDto)
        {
            await _httpClient.PutAsJsonAsync<CustomerDto>($"api/customers/{adminDto.Id}", adminDto);
            return adminDto;
        }
        public async Task<CustomerDto?> DeleteAsync(int id)
        {
            return await _httpClient.DeleteFromJsonAsync<CustomerDto?>($"api/customers/{id}");
        }
    }
}
