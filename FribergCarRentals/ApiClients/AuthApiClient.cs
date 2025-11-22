using FribergCarRentals.Core.Interfaces.ApiClients;

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
            string? currentSignedInUserId = await _httpClient.GetFromJsonAsync<string?>("api/auth/current-user-id");
            return currentSignedInUserId;
        }

        public async Task<bool> IsCustomerAsync(string userId)
        {
            bool isCustomer = await _httpClient.GetFromJsonAsync<bool>($"api/auth/is-customer/{userId}");
            return isCustomer;
        }
    }
}
