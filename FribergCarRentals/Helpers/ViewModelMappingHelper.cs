using FribergCarRentals.Models;
using FribergCarRentals.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FribergCarRentals.Helpers
{
    public static class ViewModelMappingHelper
    {
        public static void MapAToB(Admin a, AdminViewModel b)
        {
            b.Id = a.Id;
            b.IdentityUser = a.IdentityUser;
            b.FirstName = a.FirstName;
            b.LastName = a.LastName;
        }
        public static void MapAToB(AdminViewModel a, Admin b)
        {
            b.Id = a.Id;
            b.IdentityUser = a.IdentityUser;
            b.FirstName = a.FirstName;
            b.LastName = a.LastName;
        }
        public static void MapAToB(Customer a, CustomerViewModel b)
        {
            b.CustomerId = a.Id;
            if (a.IdentityUser != null)
                b.IdentityUserId = a.IdentityUser.Id;
            b.FirstName = a.FirstName;
            b.LastName = a.LastName;
            b.HomeCity = a.HomeCity;
            b.HomeCountry = a.HomeCountry;
            b.ReservationIds = a.Reservations.Select(r => r.Id).ToList();
        }
        public static void MapAToB(CustomerViewModel a, Customer b, IdentityUser identityUser, List<Reservation> reservations)
        {
            b.Id = a.CustomerId;
            b.IdentityUser = identityUser;
            b.FirstName = a.FirstName;
            b.LastName = a.LastName;
            b.HomeCity = a.HomeCity;
            b.HomeCountry = a.HomeCountry;
            b.Reservations = reservations;
        }
        public static void MapAToB(Car a, CarViewModel b)
        {
            b.Id = a.Id;
            b.Make = a.Make;
            b.Model = a.Model;
            b.Year = a.Year;
            b.Description = a.Description;
            b.ReservationIds = a.Reservations.Select(r => r.Id).ToList();
        }
        public static void MapAToB(CarViewModel a, Car b, List<Reservation> reservations)
        { 
            b.Id = a.Id;
            b.Make = a.Make;
            b.Model = a.Model;
            b.Year = a.Year;
            b.Description = a.Description;
            b.Reservations = reservations.Where(r => a.ReservationIds.Contains(r.Id)).ToList();
        }
        public static async Task MapAToB(IdentityUser a, IdentityUserViewModel b, UserManager<IdentityUser> userManager)
        {
            b.Id = a.Id;
            b.Username = a.UserName;
            b.IsAdmin = await userManager.IsInRoleAsync(a, "Admin");
            b.IsUser = await userManager.IsInRoleAsync(a, "User");
        }
        public static async Task MapAToB(IdentityUserViewModel a, IdentityUser b, UserManager<IdentityUser> userManager)
        {
            b.Id = a.Id;
            b.UserName = a.Username;
            if (a.IsAdmin)
            {
                await userManager.AddToRoleAsync(b, "Admin");
            }
            else
            {
                await userManager.RemoveFromRoleAsync(b, "Admin");
            }
            if (a.IsUser)
            {
                await userManager.AddToRoleAsync(b, "User");
            }
            else
            {
                await userManager.RemoveFromRoleAsync(b, "User");
            }
        }
        public static void MapAToB(Reservation a, ReservationViewModel b)
        {
            b.Id = a.Id;
            b.StartDate = a.StartDate;
            b.EndDate = a.EndDate;
            b.Car = a.Car;
            b.Customer = a.Customer;
        }
        public static void MapAToB(ReservationViewModel a, Reservation b)
        {
            b.Id = a.Id;
            b.StartDate = a.StartDate;
            b.EndDate = a.EndDate;
            b.Car = a.Car;
            b.Customer = a.Customer;
        }
    }
}
