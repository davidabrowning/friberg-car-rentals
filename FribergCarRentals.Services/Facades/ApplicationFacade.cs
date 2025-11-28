using FribergCarRentals.Core.Constants;
using FribergCarRentals.Core.Interfaces.Facades;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;

namespace FribergCarRentals.Services.Facades
{
    public class ApplicationFacade : IApplicationFacade
    {
        private readonly IAuthService _authService;
        private readonly IAdminService _adminService;
        private readonly ICustomerService _customerService;
        private readonly ICarService _carService;
        private readonly IReservationService _reservationService;
        private readonly IJwtService _jwtService;

        public ApplicationFacade(IAuthService authService, IAdminService adminService, ICustomerService customerService, ICarService carService, IReservationService reservationService, IJwtService jwtService)
        {
            _authService = authService;
            _adminService = adminService;
            _customerService = customerService;
            _carService = carService;
            _reservationService = reservationService;
            _jwtService = jwtService;
        }

        #region Admin

        public async Task<Admin?> GetAdminAsync(int id)
        {
            return await _adminService.GetAsync(id);
        }

        public async Task<Admin?> GetAdminAsync(string userId)
        {
            return await _adminService.GetAsync(userId);
        }

        public async Task<IEnumerable<Admin>> GetAllAdminsAsync()
        {
            return await _adminService.GetAllAsync();
        }

        public async Task<Admin> CreateAdminAsync(Admin admin)
        {
            await _authService.AddToRoleAsync(admin.UserId, AuthRoleName.Admin);
            return await _adminService.CreateAsync(admin);
        }

        public async Task<Admin> UpdateAdminAsync(Admin admin)
        {
            return await _adminService.UpdateAsync(admin);
        }

        public async Task<Admin?> DeleteAdminAsync(int id)
        {
            Admin? admin = await _adminService.GetAsync(id);
            if (admin == null)
                return null;
            string? userId = await _authService.RemoveFromRoleAsync(admin.UserId, AuthRoleName.Admin);
            return await _adminService.DeleteAsync(admin.Id);
        }

        #endregion

        #region Customer

        public async Task<Customer?> GetCustomerAsync(int id)
        {
            return await _customerService.GetAsync(id);
        }

        public async Task<Customer?> GetCustomerAsync(string userId)
        {
            return await _customerService.GetAsync(userId);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _customerService.GetAllAsync();
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            await _authService.AddToRoleAsync(customer.UserId, AuthRoleName.Customer);
            return await _customerService.CreateAsync(customer);
        }

        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            return await _customerService.UpdateAsync(customer);
        }

        public async Task<Customer?> DeleteCustomerAsync(int id)
        {
            Customer? customer = await _customerService.GetAsync(id);
            if (customer == null)
            {
                return null;
            }
            IEnumerable<Reservation> reservations = await GetReservationsByCustomerAsync(customer);
            foreach (Reservation reservation in reservations)
                await DeleteReservationAsync(reservation.Id);
            await _authService.RemoveFromRoleAsync(customer.UserId, AuthRoleName.Customer);
            return await _customerService.DeleteAsync(id);
        }

        #endregion

        #region Car

        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {
            return await _carService.GetAllAsync();
        }

        public async Task<Car?> GetCarAsync(int id)
        {
            return await _carService.GetAsync(id);
        }

        public async Task<Car> CreateCarAsync(Car car)
        {
            return await _carService.CreateAsync(car);
        }

        public async Task<Car> UpdateCarAsync(Car car)
        {
            return await _carService.UpdateAsync(car);
        }

        public async Task<Car?> DeleteCarAsync(int id)
        {
            Car? car = await _carService.GetAsync(id);
            if (car == null)
                return null;
            IEnumerable<Reservation> reservations = await GetReservationsByCarAsync(car);
            foreach (Reservation reservation in reservations)
                await DeleteReservationAsync(reservation.Id);
            return await _carService.DeleteAsync(id);
        }

        #endregion

        #region Reservation

        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            return await _reservationService.GetAllAsync();
        }

        public async Task<Reservation?> GetReservationAsync(int id)
        {
            return await _reservationService.GetAsync(id);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByCustomerAsync(Customer customer)
        {
            return await _reservationService.GetByCustomerAsync(customer);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByCarAsync(Car car)
        {
            return await _reservationService.GetByCarAsync(car);
        }

        public async Task<Reservation> CreateReservationAsync(Reservation reservation)
        {
            return await _reservationService.CreateAsync(reservation);
        }

        public async Task<Reservation> UpdateReservationAsync(Reservation reservation)
        {
            return await _reservationService.UpdateAsync(reservation);
        }

        public async Task<Reservation?> DeleteReservationAsync(int id)
        {
            return await _reservationService.DeleteAsync(id);
        }

        #endregion

        #region ApplicationUser

        public async Task<bool> UserIdExistsAsync(string id)
        {
            return await _authService.IdExistsAsync(id);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _authService.UsernameExistsAsync(username);
        }

        public async Task<string?> AuthenticateAsync(string username, string password)
        {
            return await _authService.AuthenticateUserAsync(username, password);
        }

        public async Task<string?> CreateApplicationUserAsync(string username, string password)
        {
            return await _authService.CreateUserWithPasswordAsync(username, password);
        }

        public async Task<string?> GetUsernameAsync(string userId)
        {
            return await _authService.GetUsernameAsync(userId);
        }

        public async Task<string?> GetUserIdAsync(string username)
        {
            return await _authService.GetUserIdAsync(username);
        }

        public async Task<string?> UpdateUsernameAsync(string id, string newUsername)
        {
            return await _authService.UpdateUsernameAsync(id, newUsername);
        }

        public async Task<string?> DeleteApplicationUserAsync(string userId)
        {
            if (!await UserIdExistsAsync(userId))
                return null;
            Admin? admin = await GetAdminAsync(userId);
            if (admin != null)
                await DeleteAdminAsync(admin.Id);
            Customer? customer = await GetCustomerAsync(userId);
            if (customer != null)
                await DeleteCustomerAsync(customer.Id);
            return await _authService.DeleteAsync(userId);
        }

        public async Task<IEnumerable<string>> GetAllUserIdsAsync()
        {
            return await _authService.GetAllUserIdsAsync();
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _authService.RoleExistsAsync(roleName);
        }

        public async Task<IEnumerable<string>> GetAllRolesAsync()
        {
            return await _authService.GetAllRolesAsync();
        }

        public async Task<IEnumerable<string>> GetRolesAsync(string userId)
        {
            return await _authService.GetRolesAsync(userId);
        }

        public async Task<string?> CreateRoleAsync(string roleName)
        {
            return await _authService.CreateRoleAsync(roleName);
        }

        public async Task<bool> IsInRoleAsync(string userId, string roleName)
        {
            return await _authService.IsInRoleAsync(userId, roleName);
        }

        public async Task<string?> AddToRoleAsync(string userId, string roleName)
        {
            return await _authService.AddToRoleAsync(userId, roleName);
        }

        public async Task<string?> RemoveFromRoleAsync(string userId, string roleName)
        {
            return await _authService.RemoveFromRoleAsync(userId, roleName);
        }

        #endregion

        #region Jwt

        public string GenerateJwtToken(string userId, string username, IEnumerable<string> roles)
        {
            return _jwtService.GenerateJwtToken(userId, username, roles);
        }

        #endregion
    }
}
