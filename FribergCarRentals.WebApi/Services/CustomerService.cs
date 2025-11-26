using FribergCarRentals.Core.Interfaces.Repositories;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;

namespace FribergCarRentals.WebApi.Services
{
    public class CustomerService : BasicCRUDService<Customer>, ICustomerService
    {
        private readonly IReservationService _reservationService;
        public CustomerService(IRepository<Customer> customerRepository, IReservationService reservationServices) : base(customerRepository)
        {
            _reservationService = reservationServices;
        }

        public override async Task<Customer?> DeleteAsync(int id)
        {
            Customer? customer = await GetByIdAsync(id);
            if (customer == null) {
                return null;
            }

            IEnumerable<Reservation> reservations = await _reservationService.GetByCustomerAsync(customer);
            foreach (Reservation reservation in reservations)
            {
                await _reservationService.DeleteAsync(reservation.Id);
            }

            return await base.DeleteAsync(id);
        }
    }
}
