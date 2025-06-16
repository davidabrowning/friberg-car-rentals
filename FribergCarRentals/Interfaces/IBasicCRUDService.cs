namespace FribergCarRentals.Interfaces
{
    public interface IBasicCRUDService<T>
    {
        Task<T> AddAsync(T t);
        Task<T?> DeleteAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);

        Task<bool> IdExistsAsync(int id);

        Task<T> UpdateAsync(T t);
    }
}
