namespace FribergCarRentals.Core.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateJwtToken(string userId, string username, IEnumerable<string> roles);
    }
}
