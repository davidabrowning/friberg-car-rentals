﻿using FribergCarRentals.Interfaces;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FribergCarRentals.Data
{
    public class DatabaseSeedingService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService _userService;
        private readonly ICarService _carService;
        public DatabaseSeedingService(RoleManager<IdentityRole> roleManager, IUserService userService, ICarService carService)
        {
            _roleManager = roleManager;
            _userService = userService;
            _carService = carService;
        }
        public async Task Go()
        {
            await SeedRoles();
            await SeedDefaultAdminUser();
            await SeedDefaultCars();
        }

        private async Task SeedRoles()
        {
            string[] roles = { "Admin", "Customer", "User" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private async Task SeedDefaultAdminUser()
        {
            IdentityUser? identityUser = _userService.GetAllIdentityUsersAsync()
                .Result.FirstOrDefault(iu => iu.UserName == "admin@admin.se");

            if (identityUser == null)
            {
                identityUser = await _userService.CreateUser("admin@admin.se");
            }

            Admin? admin = await _userService.GetAdminByUserAsync(identityUser);
            if (admin == null)
            {
                admin = new Admin() { IdentityUser = identityUser };
                await _userService.CreateAdminAsync(admin);
            }
        }

        private async Task SeedDefaultCars()
        {
            IEnumerable<Car> allCars = await _carService.GetAllAsync();
            if (allCars.Count() > 0)
            {
                return;
            }

            Car defaultCar1 = new()
            {
                Make = "Jeep",
                Model = "Cherokee",
                Year = 1996,
                Description = "My first car",
                PhotoUrls = new()
                {
                    "https://upload.wikimedia.org/wikipedia/commons/d/d4/1985_Jeep_Cherokee_%2814930366019%29_%28cropped%29.jpg",
                    "https://upload.wikimedia.org/wikipedia/commons/4/42/1988_Jeep_Cherokee_%28XJ%29_Pioneer_Olympic_Edition_municipal_service_Galax_Virginia_1.jpg",
                },
                Reservations = new()
            };
            await _carService.CreateAsync(defaultCar1);

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
            await _carService.CreateAsync(defaultCar2);

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
            await _carService.CreateAsync(defaultCar3);

            Car defaultCar4 = new()
            {
                Make = "Volvo",
                Model = "C70",
                Year = 2002,
                Description = "Mom's car",
                PhotoUrls = new()
                {
                    "https://upload.wikimedia.org/wikipedia/commons/0/0c/1999-2002_Volvo_C70_convertible_01.jpg",
                },
                Reservations = new()
            };
            await _carService.CreateAsync(defaultCar4);
        }
    }
}
