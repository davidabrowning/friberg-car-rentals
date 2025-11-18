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
        public async Task<IEnumerable<Car>> GetAllAsync()
        {
            List<Car> carList = await _httpClient.GetFromJsonAsync<List<Car>>("api/cars") ?? new();
            return carList;
        }
    }
}
