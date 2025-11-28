using FribergCarRentals.Core.Interfaces.Facades;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.WebApi.Mappers
{
    public static class UserMapper
    {
        public static async Task<UserDto> ToDtoAsync(string userId, IApplicationFacade applicationFacade)
        {
            AdminDto adminDto = new();
            Admin? admin = await applicationFacade.GetAdminAsync(userId);
            if (admin != null)
                adminDto = AdminMapper.ToDto(admin);

            CustomerDto customerDto = new();
            Customer? customer = await applicationFacade.GetCustomerAsync(userId);
            if (customer != null)
                customerDto = CustomerMapper.ToDto(customer);

            UserDto userDto = new()
            {
                UserId = userId,
                Username = await applicationFacade.GetUsernameAsync(userId) ?? null,
                AuthRoles = await applicationFacade.GetRolesAsync(userId),
                AdminDto = adminDto,
                CustomerDto = customerDto,
            };
            return userDto;
        }

        public static async Task<List<UserDto>> ToDtosAsync(List<string> userIds, IApplicationFacade applicationFacade)
        {
            List<UserDto> userDtos = new();
            foreach (string userId in userIds)
            {
                userDtos.Add(await ToDtoAsync(userId, applicationFacade));
            }
            return userDtos;
        }
    }
}
