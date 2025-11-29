using FribergCarRentals.Core.Interfaces.Repositories;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;

namespace FribergCarRentals.Services.Services
{
    public abstract class BasicCrudService<T> : ICrudService<T>
    {
        IRepository<T> _repository;

        public BasicCrudService(IRepository<T> repository)
        {
            _repository = repository;
        }
        public virtual async Task<T> CreateAsync(T t)
        {
            await _repository.AddAsync(t);
            return t;
        }
        public virtual async Task<T?> DeleteAsync(int id)
        {
            T? t = await GetAsync(id);
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

        public async Task<T?> GetAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> IdExistsAsync(int id)
        {
            return await _repository.IdExistsAsync(id);
        }

        public virtual async Task<T> UpdateAsync(T t)
        {
            await _repository.UpdateAsync(t);
            return t;
        }
    }
}
