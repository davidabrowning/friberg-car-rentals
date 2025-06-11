using FribergCarRentals.Areas.Administration.ViewModels;
using FribergCarRentals.Areas.CustomerCenter.ViewModels;
using FribergCarRentals.Models;
using FribergCarRentals.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentals.Areas.Administration.Helpers
{
    public static class ViewModelMappingHelper
    {
        public static AdminEditViewModel GetAdminEditViewModel(Admin admin)
        {
            return new AdminEditViewModel()
            {
                AdminId = admin.Id,
            };
        }
        public static Admin CreateNewAdmin(AdminCreateViewModel adminCreateViewModel, IdentityUser identityUser)
        {
            return new Admin()
            {
                IdentityUser = identityUser,
            };
        }
        public static CustomerEditViewModel GetCustomerEditViewModel(Customer customer)
        {
            return new CustomerEditViewModel()
            {
                CustomerId = customer.Id,
                IdentityUserId = customer.IdentityUser.Id,
                IdentityUserUsername = customer.IdentityUser.UserName,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                HomeCity = customer.HomeCity,
                HomeCountry = customer.HomeCountry,
                ReservationIds = customer.Reservations.Select(c => c.Id).ToList(),
            };
        }
        public static Customer CreateNewCustomer(CustomerCreateViewModel customerCreateViewModel, IdentityUser identityUser, List<Reservation> reservations)
        {
            return new Customer()
            {
                IdentityUser = identityUser,
                FirstName = customerCreateViewModel.FirstName,
                LastName = customerCreateViewModel.LastName,
                HomeCity = customerCreateViewModel.HomeCity,
                HomeCountry = customerCreateViewModel.HomeCountry,
                Reservations = reservations,
            };
        }
        public static void UpdatedExistingCustomer(Customer customer, CustomerEditViewModel customerEditViewModel)
        {
            customer.FirstName = customerEditViewModel.FirstName;
            customer.LastName = customerEditViewModel.LastName;
            customer.HomeCity = customerEditViewModel.HomeCity;
            customer.HomeCountry = customerEditViewModel.HomeCountry;
        }
        public static CarViewModel GetCarViewModel(Car car)
        {
            return new CarViewModel()
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                Description = car.Description,
                ReservationIds = car.Reservations.Select(r => r.Id).ToList(),
            };
        }
        public static Car CreateNewCar(CarViewModel carViewModel, List<Reservation> reservations)
        {
            return new Car()
            {
                Make = carViewModel.Make,
                Model = carViewModel.Model,
                Year = carViewModel.Year,
                Description = carViewModel.Description,
                Reservations = reservations,
            };
        }
        public static IdentityUserEditViewModel GetIdentityUserEditViewModel(IdentityUser user)
        {
            return new IdentityUserEditViewModel()
            {
                IdentityUserId = user.Id,
                IdentityUserUsername = user.UserName,
            };
        }
        public static async Task<IdentityUserViewModel> GetIdentityUserViewModel(IdentityUser identityUser, UserManager<IdentityUser> userManager)
        {
            return new IdentityUserViewModel()
            {
                Id = identityUser.Id,
                Username = identityUser.UserName,
                IsAdmin = await userManager.IsInRoleAsync(identityUser, "Admin"),
                IsCustomer = await userManager.IsInRoleAsync(identityUser, "Customer"),
                IsUser = await userManager.IsInRoleAsync(identityUser, "User"),
            };
        }
        public static IdentityUserDeleteViewModel GetIdentityUserDeleteViewModel(IdentityUser identityUser)
        {
            return new IdentityUserDeleteViewModel()
            {
                IdentityUserId = identityUser.Id,
                IdentityUserUsername = identityUser.UserName ?? "",
            };
        }
        public static async Task<IdentityUser> GetIdentityUser(IdentityUserEditViewModel identityUserEditViewModel, UserManager<IdentityUser> userManager)
        {
            return await userManager.Users.Where(u => u.Id == identityUserEditViewModel.IdentityUserId).FirstOrDefaultAsync();
        }
        public static ReservationViewModel GetReservationViewModel(Reservation reservation)
        {
            return new ReservationViewModel()
            {
                Id = reservation.Id,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                Car = reservation.Car,
                Customer = reservation.Customer,
            };
        }
        public static Reservation GetReservation(ReservationViewModel reservationViewModel)
        {
            return new Reservation()
            {
                Id = reservationViewModel.Id,
                StartDate = reservationViewModel.StartDate,
                EndDate = reservationViewModel.EndDate,
                Car = reservationViewModel.Car,
                Customer = reservationViewModel.Customer,
            };
        }
    }
}
