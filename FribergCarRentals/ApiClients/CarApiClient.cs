using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.Core.Models;

namespace FribergCarRentals.Mvc.ApiClients
{
    public class CarApiClient : IApiClient<Car>
    {
        private readonly HttpClient _httpClient;
        public CarApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Car>> GetAsync()
        {
            List<Car> carList = await _httpClient.GetFromJsonAsync<List<Car>>("api/cars") ?? new();
            return carList;
        }

        public async Task<Car?> GetAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Car?>($"api/cars/{id}");
        }

        public async Task<Car> PostAsync(Car car)
        {
            await _httpClient.PostAsJsonAsync<Car>("api/cars", car);
            return car;
        }

        public async Task PutAsync(Car car)
        {
            await _httpClient.PutAsJsonAsync<Car>($"api/cars/{car.Id}", car);
        }

        public async Task DeleteAsync(int id)
        {
            Car? deletedCar = await _httpClient.DeleteFromJsonAsync<Car?>($"api/cars/{id}");
        }
    }
}
