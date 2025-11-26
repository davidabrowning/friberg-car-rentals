namespace FribergCarRentals.Core.Interfaces.Services
{
    public interface ICrudService<T>
    {
        Task<T> CreateAsync(T t);
        Task<T?> DeleteAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<bool> IdExistsAsync(int id);
        Task<T> UpdateAsync(T t);
    }
}
