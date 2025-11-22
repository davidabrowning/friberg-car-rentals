using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.Mvc.ApiClients
{
    public class AuthApiClient : IAuthApiClient
    {
        private readonly HttpClient _httpClient;
        public AuthApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string?> GetCurrentSignedInUserIdAsync()
        {
            SignedInUserDto signedInUserDto = await _httpClient.GetFromJsonAsync<SignedInUserDto>("api/auth/current-user-id");
            return signedInUserDto.UserId;
        }

        public async Task<bool> IsCustomerAsync(string userId)
        {
            bool isCustomer = await _httpClient.GetFromJsonAsync<bool>($"api/auth/is-customer/{userId}");
            return isCustomer;
        }
    }
}
