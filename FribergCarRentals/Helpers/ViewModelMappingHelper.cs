using FribergCarRentals.Models;
using FribergCarRentals.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FribergCarRentals.Helpers
{
    public static class ViewModelMappingHelper
    {
        public static void MapAToB(Customer a, CustomerViewModel b)
        {
            b.Id = a.Id;
            b.ApplicationUser = a.ApplicationUser;
            b.FirstName = a.FirstName;
            b.LastName = a.LastName;
            b.HomeCity = a.HomeCity;
            b.HomeCountry = a.HomeCountry;
            b.Reservations = a.Reservations;
        }
        public static void MapAToB(CustomerViewModel a, Customer b)
        {
            b.Id = a.Id;
            b.ApplicationUser = a.ApplicationUser;
            b.FirstName = a.FirstName;
            b.LastName = a.LastName;
            b.HomeCity = a.HomeCity;
            b.HomeCountry = a.HomeCountry;
            b.Reservations = a.Reservations;
        }
        public static void MapAToB(Car a, CarViewModel b)
        {
            b.Id = a.Id;
            b.Make = a.Make;
            b.Model = a.Model;
            b.Year = a.Year;
            b.Description = a.Description;
            b.Reservations = a.Reservations;
        }
        public static void MapAToB(CarViewModel a, Car b)
        { 
            b.Id = a.Id;
            b.Make = a.Make;
            b.Model = a.Model;
            b.Year = a.Year;
            b.Description = a.Description;
            b.Reservations = a.Reservations;
        }
        public static async Task MapAToB(ApplicationUser a, ApplicationUserViewModel b, UserManager<ApplicationUser> userManager)
        {
            b.Id = a.Id;
            b.Username = a.UserName;
            b.IsAdmin = await userManager.IsInRoleAsync(a, "Admin");
            b.IsUser = await userManager.IsInRoleAsync(a, "User");
        }
        public static async Task MapAToB(ApplicationUserViewModel a, ApplicationUser b, UserManager<ApplicationUser> userManager)
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
    }
}
