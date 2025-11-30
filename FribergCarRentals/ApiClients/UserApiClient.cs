using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.Mvc.ApiClients
{
    public class UserApiClient : IUserApiClient
    {
        private readonly HttpClient _httpClient;
        private const string _apiPath = "api/users";
        public UserApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<UserDto>> GetAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_apiPath);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToFetchDataFromApi);
            IEnumerable<UserDto>? dtos = await response.Content.ReadFromJsonAsync<IEnumerable<UserDto>>();
            if (dtos == null)
                throw new InvalidDataException(UserMessage.ErrorResultIsNullfromApi);
            return dtos;
        }

        public async Task<UserDto> GetAsync(string userId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_apiPath}/{userId}");
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToFetchDataFromApi);
            UserDto? dto = await response.Content.ReadFromJsonAsync<UserDto>();
            if (dto == null)
                throw new InvalidDataException(UserMessage.ErrorResultIsNullfromApi);
            return dto;
        }

        public async Task<UserDto> GetByUsernameAsync(string username)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_apiPath}/username/{username}");
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToFetchDataFromApi);
            UserDto? userDto = await response.Content.ReadFromJsonAsync<UserDto>();
            if (userDto == null)
                throw new InvalidDataException(UserMessage.ErrorResultIsNullfromApi);
            return userDto;
        }

        public async Task DeleteUserAsync(string userId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{_apiPath}/{userId}");
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToSendDataToApi);
        }

        public async Task<JwtTokenDto> LoginAsync(string username, string password)
        {
            LoginDto loginDto = new LoginDto() { 
                Username = username, 
                Password = password 
            };
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{_apiPath}/login", loginDto);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToSignIn);
            JwtTokenDto? jwtTokenDto = await response.Content.ReadFromJsonAsync<JwtTokenDto>();
            if (jwtTokenDto == null)
                throw new InvalidDataException(UserMessage.ErrorResultIsNullfromApi);
            return jwtTokenDto;

        }

        public async Task<UserDto> RegisterAsync(string username, string password)
        {
            RegisterDto registerDto = new() { Username = username, Password = password };
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<RegisterDto>($"{_apiPath}/register", registerDto);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToRegister);
            UserDto? userDto = await response.Content.ReadFromJsonAsync<UserDto>();
            if (userDto == null)
                throw new InvalidDataException(UserMessage.ErrorResultIsNullfromApi);
            return userDto;
        }

        public async Task UpdateUsernameAsync(string userId, string newUsername)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{_apiPath}/update-username/{userId}", newUsername);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(UserMessage.ErrorUnableToSendDataToApi);
        }
    }
}
