using FribergCarRentals.Core.Constants;
using FribergCarRentals.Core.Interfaces.Facades;
using FribergCarRentals.Core.Interfaces.Other;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace FribergCarRentals.Data
{
    public class DatabaseSeedingService : IDatabaseSeeder
    {
        private IApplicationFacade _applicationFacade;
        public DatabaseSeedingService(IApplicationFacade applicationFacade)
        {
            _applicationFacade = applicationFacade;
        }
        public async Task SeedAsync()
        {
            await SeedRoles();
            await SeedDefaultAdminUser();
            await SeedDefaultCars();
        }

        private async Task SeedRoles()
        {
            List<string> roleNames = new() { AuthRoleName.Admin, AuthRoleName.Customer, AuthRoleName.User };
            foreach (string roleName in roleNames)
            {
                if (!await _applicationFacade.RoleExistsAsync(roleName))
                {
                    await _applicationFacade.CreateRoleAsync(roleName);
                }
            }
        }

        private async Task SeedDefaultAdminUser()
        {
            string defaultAdminUsername = "admin@admin.se";
            string defaultAdminPassword = "Abc123!";
            string? defaultAdminUserId = await _applicationFacade.GetUserIdAsync(defaultAdminUsername);
            if (defaultAdminUserId == null)
            {
                defaultAdminUserId = await _applicationFacade.CreateApplicationUserAsync(defaultAdminUsername, defaultAdminPassword);
            }

            Admin? admin = await _applicationFacade.GetAdminAsync(defaultAdminUserId);
            if (admin == null)
            {
                admin = new Admin() { UserId = defaultAdminUserId };
                await _applicationFacade.CreateAdminAsync(admin);
            }
        }

        private async Task SeedDefaultCars()
        {
            IEnumerable<Car> allCars = await _applicationFacade.GetAllCarsAsync();
            if (allCars.Count() > 0)
            {
                return;
            }

            Car defaultCar1 = new()
            {
                Make = "Jeep",
                Model = "Cherokee",
                Year = 1996,
                Description = "David's first car",
                PhotoUrls = new()
                {
                    "https://upload.wikimedia.org/wikipedia/commons/d/d4/1985_Jeep_Cherokee_%2814930366019%29_%28cropped%29.jpg",
                    "https://upload.wikimedia.org/wikipedia/commons/4/42/1988_Jeep_Cherokee_%28XJ%29_Pioneer_Olympic_Edition_municipal_service_Galax_Virginia_1.jpg",
                },
                Reservations = new()
            };
            await _applicationFacade.CreateCarAsync(defaultCar1);

            Car defaultCar2 = new()
            {
                Make = "Toyota",
                Model = "RAV4",
                Year = 1995,
                Description = "Anneli's first car",
                PhotoUrls = new()
                {
                    "https://upload.wikimedia.org/wikipedia/commons/6/6f/1995_Toyota_RAV4_%28SXA11R%29_Cruiser_wagon_%282015-07-14%29_02.jpg",
                    "https://upload.wikimedia.org/wikipedia/commons/2/25/1996_Toyota_RAV4_Max_2.0_Rear.jpg",
                },
                Reservations = new()
            };
            await _applicationFacade.CreateCarAsync(defaultCar2);

            Car defaultCar3 = new()
            {
                Make = "Honda",
                Model = "CRV",
                Year = 2000,
                Description = "Steph's first car",
                PhotoUrls = new()
                {
                    "https://upload.wikimedia.org/wikipedia/commons/1/1b/Honda_CR-V_e-HEV_Elegance_AWD_%28VI%29_%E2%80%93_f_14072024.jpg",
                    "https://upload.wikimedia.org/wikipedia/commons/d/de/1996_Honda_CR-V_%28RD1%29%2C_front_right.jpg",
                },
                Reservations = new()
            };
            await _applicationFacade.CreateCarAsync(defaultCar3);

            Car defaultCar4 = new()
            {
                Make = "Volvo",
                Model = "C70",
                Year = 2002,
                Description = "Cecilia's first car",
                PhotoUrls = new()
                {
                    "https://upload.wikimedia.org/wikipedia/commons/0/0c/1999-2002_Volvo_C70_convertible_01.jpg",
                },
                Reservations = new()
            };
            await _applicationFacade.CreateCarAsync(defaultCar4);
        }
    }
}
