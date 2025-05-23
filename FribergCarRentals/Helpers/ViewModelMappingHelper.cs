using FribergCarRentals.Models;
using FribergCarRentals.ViewModels;

namespace FribergCarRentals.Helpers
{
    public static class ViewModelMappingHelper
    {
        public static void MapAToB(Customer a, CustomerViewModel b)
        {
            b.Id = a.Id;
            b.FirstName = a.FirstName;
            b.LastName = a.LastName;
            b.HomeCity = a.HomeCity;
            b.HomeCountry = a.HomeCountry;
        }
        public static void MapAToB(CustomerViewModel a, Customer b)
        {
            b.Id = a.Id;
            b.FirstName = a.FirstName;
            b.LastName = a.LastName;
            b.HomeCity = a.HomeCity;
            b.HomeCountry = a.HomeCountry;
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
    }
}
