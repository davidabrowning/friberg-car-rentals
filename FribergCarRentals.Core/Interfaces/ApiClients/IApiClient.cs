namespace FribergCarRentals.Core.Interfaces.ApiClients
{
    public interface IApiClient<T> where T : class
    {
        Task<IEnumerable<T>> GetAsync();
        Task<T?> GetAsync(int id);
        Task<T> PostAsync(T t);
        Task PutAsync(T t);
        Task DeleteAsync(int id);
    }
}
