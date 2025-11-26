using FribergCarRentals.Core.Interfaces.Repositories;
using FribergCarRentals.Core.Interfaces.Services;

namespace FribergCarRentals.WebApi.Services
{
    public abstract class BasicCRUDService<T> : IBasicCRUDService<T>
    {
        IRepository<T> _repository;

        public BasicCRUDService(IRepository<T> repository)
        {
            _repository = repository;
        }
        public async Task<T> CreateAsync(T t)
        {
            await _repository.AddAsync(t);
            return t;
        }
        public virtual async Task<T?> DeleteAsync(int id)
        {
            T? t = await GetByIdAsync(id);
            if (t == null)
            {
                return t;
            }

            await _repository.DeleteAsync(id);
            return t;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> IdExistsAsync(int id)
        {
            return await _repository.IdExistsAsync(id);
        }

        public async Task<T> UpdateAsync(T t)
        {
            await _repository.UpdateAsync(t);
            return t;
        }
    }
}
