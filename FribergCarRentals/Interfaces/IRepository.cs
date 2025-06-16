namespace FribergCarRentals.Interfaces
{
    public interface IRepository<T>
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T t);
        Task UpdateAsync(T t);
        Task DeleteAsync(int id);
        Task<bool> IdExistsAsync(int id);
    }
}
