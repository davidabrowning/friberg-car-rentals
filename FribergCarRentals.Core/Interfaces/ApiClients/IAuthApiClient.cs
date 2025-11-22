namespace FribergCarRentals.Core.Interfaces.ApiClients
{
    public interface IAuthApiClient
    {
        Task<string?> GetCurrentSignedInUserIdAsync();
        Task<bool> IsCustomerAsync(string userId);
    }
}
