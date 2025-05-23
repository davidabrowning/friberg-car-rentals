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
    }
}
