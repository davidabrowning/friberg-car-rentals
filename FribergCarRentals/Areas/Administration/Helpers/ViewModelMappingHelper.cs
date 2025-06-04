using FribergCarRentals.Areas.Administration.ViewModels;
using FribergCarRentals.Models;
using FribergCarRentals.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace FribergCarRentals.Areas.Administration.Helpers
{
    public static class ViewModelMappingHelper
    {
        public static AdminViewModel GetAdminViewModel(Admin admin)
        {
            return new AdminViewModel()
            {
                Id = admin.Id,
                IdentityUserId = admin.IdentityUser.Id,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
            };
        }
        public static Admin GetAdmin(AdminViewModel adminViewModel, IdentityUser identityUser)
        {
            return new Admin()
            {
                Id = adminViewModel.Id,
                IdentityUser = identityUser,
                FirstName = adminViewModel.FirstName,
                LastName = adminViewModel.LastName,
            };
        }
        public static CustomerViewModel GetCustomerViewModel(Customer customer)
        {
            return new CustomerViewModel()
            {
                CustomerId = customer.Id,
                IdentityUserId = customer.IdentityUser.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                HomeCity = customer.HomeCity,
                HomeCountry = customer.HomeCountry,
                ReservationIds = customer.Reservations.Select(c => c.Id).ToList(),
            };
        }
        public static Customer GetCustomer(CustomerViewModel customerViewModel, IdentityUser identityUser, List<Reservation> reservations)
        {
            return new Customer()
            {
                Id = customerViewModel.CustomerId,
                IdentityUser = identityUser,
                FirstName = customerViewModel.FirstName,
                LastName = customerViewModel.LastName,
                HomeCity = customerViewModel.HomeCity,
                HomeCountry = customerViewModel.HomeCountry,
                Reservations = reservations
            };
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
        public static Car GetCar(CarViewModel carViewModel, List<Reservation> reservations)
        {
            return new Car()
            {
                Id = carViewModel.Id,
                Make = carViewModel.Make,
                Model = carViewModel.Model,
                Year = carViewModel.Year,
                Description = carViewModel.Description,
                Reservations = reservations,
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
        public static async Task<IdentityUser> GetIdentityUser(IdentityUserViewModel identityUserViewModel, UserManager<IdentityUser> userManager)
        {
            IdentityUser identityUser = userManager.Users.Where(u => u.Id == identityUserViewModel.Id).FirstOrDefault();
            if (identityUserViewModel.IsAdmin)
            {
                await userManager.AddToRoleAsync(identityUser, "Admin");
            }
            else
            {
                await userManager.RemoveFromRoleAsync(identityUser, "Admin");
            }
            if (identityUserViewModel.IsCustomer)
            {
                await userManager.AddToRoleAsync(identityUser, "Customer");
            }
            else
            {
                await userManager.RemoveFromRoleAsync(identityUser, "Customer");
            }
            if (identityUserViewModel.IsUser)
            {
                await userManager.AddToRoleAsync(identityUser, "User");
            }
            else
            {
                await userManager.RemoveFromRoleAsync(identityUser, "User");
            }
            return identityUser;
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
