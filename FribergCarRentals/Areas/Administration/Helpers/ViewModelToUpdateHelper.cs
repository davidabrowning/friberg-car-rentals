using FribergCarRentals.Areas.Administration.ViewModels;
using FribergCarRentals.Models;

namespace FribergCarRentals.Areas.Administration.Helpers
{
    public static class ViewModelToUpdateHelper
    {
        public static void UpdatedExistingCustomer(Customer customer, CustomerEditViewModel customerEditViewModel)
        {
            customer.FirstName = customerEditViewModel.FirstName;
            customer.LastName = customerEditViewModel.LastName;
            customer.HomeCity = customerEditViewModel.HomeCity;
            customer.HomeCountry = customerEditViewModel.HomeCountry;
        }
        public static void UpdateExistingCar(Car car, CarEditViewModel carEditViewModel)
        {
            car.Make = carEditViewModel.Make;
            car.Model = carEditViewModel.Model;
            car.Year = carEditViewModel.Year;
            car.Description = carEditViewModel.Description;
        }
    }
}
