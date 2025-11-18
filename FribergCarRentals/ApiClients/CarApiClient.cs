using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.Core.Models;

namespace FribergCarRentals.Mvc.ApiClients
{
    public class CarApiClient : ICarApiClient
    {
        private readonly HttpClient _httpClient;
        public CarApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Car> CreateAsync(Car car)
        {
            await _httpClient.PostAsJsonAsync<Car>("api/cars", car);
            return car;
        }

        public async Task<Car?> DeleteAsync(int id)
        {
            Car? deletedCar = await _httpClient.DeleteFromJsonAsync<Car?>($"api/cars/{id}");
            return deletedCar;
        }

        public async Task<IEnumerable<Car>> GetAllAsync()
        {
            List<Car> carList = await _httpClient.GetFromJsonAsync<List<Car>>("api/cars") ?? new();
            return carList;
        }
        public async Task<Car?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Car?>($"api/cars/{id}");
        }

        public async Task<bool> IdExistsAsync(int id)
        {
            Car? car = await GetByIdAsync(id);
            return car != null;
        }

        public async Task<Car> UpdateAsync(Car car)
        {
            await _httpClient.PutAsJsonAsync<Car>($"api/cars/{car.Id}", car);
            return car;
        }
    }
}
