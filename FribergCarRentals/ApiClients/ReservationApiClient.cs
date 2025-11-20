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

        public async Task<IEnumerable<Reservation>> GetAsync()
        {
            List<Reservation> reservationList = await _httpClient.GetFromJsonAsync<List<Reservation>>("api/reservations") ?? new();
            return reservationList;
        }

        public async Task<Reservation?> GetAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Reservation>($"api/reservations/{id}");
        }

        public async Task<Reservation> PostAsync(Reservation reservation)
        {
            await _httpClient.PostAsJsonAsync<Reservation>("api/reservations", reservation);
            return reservation;
        }

        public async Task PutAsync(Reservation reservation)
        {
            await _httpClient.PutAsJsonAsync($"api/reservations/{reservation.Id}", reservation);
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClient.DeleteAsync($"api/reservations/{id}");
        }
    }
}
