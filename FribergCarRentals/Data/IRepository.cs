namespace FribergCarRentals.Data
{
    public interface IRepository<T>
    {
        Task<T?> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task Add(T t);
        Task Update(T t);
        Task Delete(int id);
        Task<bool> IdExists(int id);
    }
}
