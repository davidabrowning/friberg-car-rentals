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
            Customer? customer = await GetAsync(id);
            if (customer == null) {
                return null;
            }
            await _reservationService.DeleteByCustomerAsync(customer);
            return await base.DeleteAsync(id);
        }
    }
}
