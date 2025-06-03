using FribergCarRentals.Models;

namespace FribergCarRentals.Data
{
    public interface ICar
    {
        void Add(Car car);
        bool IdExists(int id);
        IEnumerable<Car> GetAll();
        Car GetById(int id);
        void Delete(int id);
        void Update(Car car);
    }
}
