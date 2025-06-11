using FribergCarRentals.Areas.Administration.ViewModels;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity;

namespace FribergCarRentals.Areas.Administration.Helpers
{
    public static class ViewModelToCreateHelper
    {
        public static Admin CreateNewAdmin(AdminCreateViewModel adminCreateViewModel, IdentityUser identityUser)
        {
            return new Admin()
            {
                IdentityUser = identityUser,
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
        public static Car CreateNewCar(CarCreateViewModel carCreateViewModel)
        {
            return new Car()
            {
                Make = carCreateViewModel.Make,
                Model = carCreateViewModel.Model,
                Year = carCreateViewModel.Year,
                Description = carCreateViewModel.Description,
            };
        }
        public static Reservation GetReservation(ReservationIndexViewModel reservationViewModel)
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
