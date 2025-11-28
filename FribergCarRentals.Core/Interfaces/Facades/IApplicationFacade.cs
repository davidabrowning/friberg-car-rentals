using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.Core.Interfaces.Facades
{
    public interface IApplicationFacade
    {
        // Admin
        Task<IEnumerable<Admin>> GetAllAdminsAsync();
        Task<Admin?> GetAdminAsync(int id);
        Task<Admin?> GetAdminAsync(string userId);
        Task<Admin> CreateAdminAsync(Admin admin);
        Task<Admin> UpdateAdminAsync(Admin admin);
        Task<Admin?> DeleteAdminAsync(int id);

        // Customer
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerAsync(int id);
        Task<Customer?> GetCustomerAsync(string userId);
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<Customer> UpdateCustomerAsync(Customer customer);
        Task<Customer?> DeleteCustomerAsync(int id);

        // Car
        Task<IEnumerable<Car>> GetAllCarsAsync();
        Task<Car?> GetCarAsync(int id);
        Task<Car> CreateCarAsync(Car car);
        Task<Car> UpdateCarAsync(Car car);
        Task<Car?> DeleteCarAsync(int id);

        // Reservation
        Task<IEnumerable<Reservation>> GetAllReservationsAsync();
        Task<Reservation?> GetReservationAsync(int id);
        Task<IEnumerable<Reservation>> GetReservationsByCustomerAsync(Customer customer);
        Task<IEnumerable<Reservation>> GetReservationsByCarAsync(Car car);
        Task<Reservation> CreateReservationAsync(Reservation reservation);
        Task<Reservation> UpdateReservationAsync(Reservation reservation);
        Task<Reservation?> DeleteReservationAsync(int id);

        // ApplicationUser
        Task<bool> UserIdExistsAsync(string userId);
        Task<bool> UsernameExistsAsync(string username);
        Task<string?> AuthenticateAsync(string username, string password);
        Task<string?> CreateApplicationUserAsync(string username, string password);
        Task<string?> GetUsernameAsync(string userId);
        Task<string?> GetUserIdAsync(string username);
        Task<string?> UpdateUsernameAsync(string userId, string newUsername);
        Task<string?> DeleteApplicationUserAsync(string userId);
        Task<IEnumerable<string>> GetAllUserIdsAsync();
        Task<bool> RoleExistsAsync(string roleName);
        Task<IEnumerable<string>> GetAllRolesAsync();
        Task<IEnumerable<string>> GetRolesAsync(string userId);
        Task<string?> CreateRoleAsync(string roleName);
        Task<bool> IsInRoleAsync(string userId, string roleName);
        Task<string?> AddToRoleAsync(string userId, string roleName);
        Task<string?> RemoveFromRoleAsync(string userId, string roleName);

        // Jwt
        string GenerateJwtToken(string userId, string username, IEnumerable<string> roles);
    }
}
