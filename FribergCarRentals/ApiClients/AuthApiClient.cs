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

        public async Task CreateUserAsync(string username)
        {
            await _httpClient.PostAsJsonAsync("api/auth/create-user", username);
        }

        public async Task DeleteUserAsync(string userId)
        {
            await _httpClient.DeleteAsync($"api/auth/delete-user/{userId}");
        }

        public async Task<int> GetAdminIdByUserId(string userId)
        {
            return await _httpClient.GetFromJsonAsync<int>($"api/auth/admin-id/{userId}");
        }

        public async Task<List<string>> GetAllUserIdsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<string>>($"api/auth/get-all-user-ids");
        }

        public async Task<string?> GetCurrentSignedInUserIdAsync()
        {
            SignedInUserDto signedInUserDto = await _httpClient.GetFromJsonAsync<SignedInUserDto>("api/auth/current-user-id");
            return signedInUserDto.UserId;
        }

        public async Task<int> GetCustomerIdByUserId(string userId)
        {
            return await _httpClient.GetFromJsonAsync<int>($"api/auth/customer-id/{userId}");
        }

        public async Task<string> GetUsernameByUserIdAsync(string userId)
        {
            return await _httpClient.GetStringAsync($"api/auth/username/{userId}");
        }

        public async Task<bool> IsAdminAsync(string userId)
        {
            bool isAdmin = await _httpClient.GetFromJsonAsync<bool>($"api/auth/is-admin/{userId}");
            return isAdmin;
        }

        public async Task<bool> IsCustomerAsync(string userId)
        {
            bool isCustomer = await _httpClient.GetFromJsonAsync<bool>($"api/auth/is-customer/{userId}");
            return isCustomer;
        }

        public async Task<bool> IsInRoleAsync(string userId, string roleName)
        {
            return await _httpClient.GetFromJsonAsync<bool>($"api/auth/is-in-role/{userId}/{roleName}");
        }

        public async Task<bool> IsUserAsync(string userId)
        {
            bool isUser = await _httpClient.GetFromJsonAsync<bool>($"api/auth/is-user/{userId}");
            return isUser;
        }

        public async Task UpdateUsernameAsync(string userId, string newUsername)
        {
            await _httpClient.PutAsJsonAsync<string>($"api/auth/update-username/{userId}", newUsername);
        }
    }
}
