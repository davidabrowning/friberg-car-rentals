using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.Core.Models;

namespace FribergCarRentals.Mvc.ApiClients
{
    public class ReservationApiClient : IApiClient<Reservation>
    {
        private readonly HttpClient _httpClient;
        public ReservationApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Reservation>> GetAsync()
        {
            List<Reservation> reservationList = await _httpClient.GetFromJsonAsync<List<Reservation>>("api/reservations") ?? new();
            return reservationList;
        }

        public async Task<Reservation?> GetAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Reservation>($"api/reservations/{id}");
        }

        public Task<Reservation> PostAsync(Reservation t)
        {
            throw new NotImplementedException();
        }

        public Task PutAsync(Reservation t)
        {
            throw new NotImplementedException();
        }
    }
}
