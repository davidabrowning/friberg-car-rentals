using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.Mvc.ApiClients
{
    public class UserApiClient : IUserApiClient
    {
        private readonly HttpClient _httpClient;
        public UserApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UserDto>> GetAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<UserDto>>("api/users") ?? new();
        }

        public async Task<UserDto> GetAsync(string userId)
        {
            return await _httpClient.GetFromJsonAsync<UserDto>($"api/users/{userId}");
        }

        public async Task<UserDto> GetByUsernameAsync(string username)
        {
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"api/users/username/{username}");
            if (!httpResponseMessage.IsSuccessStatusCode)
                return new UserDto();
            UserDto userDto = await httpResponseMessage.Content.ReadFromJsonAsync<UserDto>();
            return userDto;
        }

        public async Task CreateUserFromUsernameAsync(string username)
        {
            await _httpClient.PostAsJsonAsync("api/users/create-from-username", username);
        }

        public async Task DeleteUserAsync(string userId)
        {
            await _httpClient.DeleteAsync($"api/users/{userId}");
        }

        public async Task<JwtTokenDto?> LoginAsync(string username, string password)
        {
            LoginDto loginDto = new LoginDto() { 
                Username = username, 
                Password = password 
            };
            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync<LoginDto>("api/users/login", loginDto);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                return null;
            }
            return await httpResponseMessage.Content.ReadFromJsonAsync<JwtTokenDto>();

        }

        public async Task<UserDto> RegisterAsync(string username, string password)
        {
            RegisterDto registerDto = new() { Username = username, Password = password };
            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync<RegisterDto>("api/users/register", registerDto);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                return null;
            }
            UserDto userDto = await httpResponseMessage.Content.ReadFromJsonAsync<UserDto>();
            return userDto;
        }

        public async Task UpdateUsernameAsync(string userId, string newUsername)
        {
            await _httpClient.PutAsJsonAsync<string>($"api/users/update-username/{userId}", newUsername);
        }
    }
}
