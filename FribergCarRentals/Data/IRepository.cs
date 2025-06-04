namespace FribergCarRentals.Data
{
    public interface IRepository<T>
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Add(T t);
        void Update(T t);
        void Delete(int id);
        bool IdExists(int id);
    }
}
