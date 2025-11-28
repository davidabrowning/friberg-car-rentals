using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.Core.Interfaces.ApiClients
{
    public interface IUserApiClient
    {
        Task<List<UserDto>> GetAsync();
        Task<UserDto> GetAsync(string userId);
        Task<UserDto> GetByUsernameAsync(string username);
        Task<JwtTokenDto> LoginAsync(string username, string password);
        Task<UserDto> RegisterAsync(string username, string password);
        Task UpdateUsernameAsync(string userId, string newUsername);
        Task DeleteUserAsync(string userId);
    }
}
