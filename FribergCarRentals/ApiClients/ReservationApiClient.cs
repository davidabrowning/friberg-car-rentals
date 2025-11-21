using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.Core.Models;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.Mvc.ApiClients
{
    public class ReservationApiClient : IApiClient<ReservationDto>
    {
        private readonly HttpClient _httpClient;
        public ReservationApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ReservationDto>> GetAsync()
        {
            List<ReservationDto> reservationDtos = await _httpClient.GetFromJsonAsync<List<ReservationDto>>("api/reservations") ?? new();
            return reservationDtos;
        }

        public async Task<ReservationDto?> GetAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<ReservationDto>($"api/reservations/{id}");
        }

        public async Task<ReservationDto> PostAsync(ReservationDto reservationDto)
        {
            await _httpClient.PostAsJsonAsync<ReservationDto>("api/reservations", reservationDto);
            return reservationDto;
        }

        public async Task PutAsync(ReservationDto reservationDto)
        {
            await _httpClient.PutAsJsonAsync($"api/reservations/{reservationDto.Id}", reservationDto);
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClient.DeleteAsync($"api/reservations/{id}");
        }
    }
}
