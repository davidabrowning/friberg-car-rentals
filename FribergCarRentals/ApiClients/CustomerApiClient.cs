using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.Mvc.ApiClients
{
    public class CustomerApiClient : ICRUDApiClient<CustomerDto>
    {
        private readonly HttpClient _httpClient;
        private const string _apiPath = "api/cars";
        public CustomerApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CustomerDto>> GetAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_apiPath);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToFetchDataFromApi);
            IEnumerable<CustomerDto>? dtos = await response.Content.ReadFromJsonAsync<IEnumerable<CustomerDto>>();
            if (dtos == null)
                throw new InvalidDataException(UserMessage.ErrorResultIsNullfromApi);
            return dtos;
        }

        public async Task<CustomerDto> GetAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_apiPath}/{id}");
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToFetchDataFromApi);
            CustomerDto? dto = await response.Content.ReadFromJsonAsync<CustomerDto>();
            if (dto == null)
                throw new InvalidDataException(UserMessage.ErrorResultIsNullfromApi);
            return dto;
        }

        public async Task<CustomerDto> PostAsync(CustomerDto dto)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_apiPath, dto);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToSendDataToApi);
            return dto;
        }

        public async Task<CustomerDto> PutAsync(CustomerDto dto)
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
