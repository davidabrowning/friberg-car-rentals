using FribergCarRentals.Areas.Administration.ViewModels;
using FribergCarRentals.Areas.CustomerCenter.ViewModels;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentals.Areas.Administration.Helpers
{
    public static class ViewModelMakerHelper
    {
        public static AdminEditViewModel MakeAdminEditViewModel(Admin admin)
        {
            return new AdminEditViewModel()
            {
                AdminId = admin.Id,
            };
        }
        public static CustomerEditViewModel MakeCustomerEditViewModel(Customer customer)
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
        public static CarIndexViewModel MakeCarIndexViewModel(Car car)
        {
            return new CarIndexViewModel()
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                Description = car.Description,
                ReservationIds = car.Reservations.Select(r => r.Id).ToList(),
            };
        }
        public static CarEditViewModel MakeCarEditViewModel(Car car)
        {
            return new CarEditViewModel()
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                Description = car.Description,
            };
        }
        public static IdentityUserEditViewModel MakeIdentityUserEditViewModel(IdentityUser user)
        {
            return new IdentityUserEditViewModel()
            {
                IdentityUserId = user.Id,
                IdentityUserUsername = user.UserName,
            };
        }
        public static IdentityUserDeleteViewModel MakeIdentityUserDeleteViewModel(IdentityUser identityUser)
        {
            return new IdentityUserDeleteViewModel()
            {
                IdentityUserId = identityUser.Id,
                IdentityUserUsername = identityUser.UserName ?? "",
            };
        }
        public static ReservationIndexViewModel MakeReservationIndexViewModel(Reservation reservation)
        {
            return new ReservationIndexViewModel()
            {
                Id = reservation.Id,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                Car = reservation.Car,
                Customer = reservation.Customer,
            };
        }
    }
}
