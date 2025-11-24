using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.WebApi.Mappers
{
    public static class UserMapper
    {
        public static async Task<UserDto> ToDtoAsync(string userId, IUserService userService, IAuthService authService)
        {
            AdminDto adminDto = new();
            Admin? admin = await userService.GetAdminByUserIdAsync(userId);
            if (admin != null)
                adminDto = AdminMapper.ToDto(admin);

            CustomerDto customerDto = new();
            Customer? customer = await userService.GetCustomerByUserIdAsync(userId);
            if (customer != null)
                customerDto = CustomerMapper.ToDto(customer);

            UserDto userDto = new()
            {
                UserId = userId,
                Username = await userService.GetUsernameByUserIdAsync(userId) ?? null,
                AuthRoles = await authService.GetRolesAsync(userId),
                AdminDto = adminDto,
                CustomerDto = customerDto,
            };
            return userDto;
        }

        public static string ToUserId(UserDto userDto)
        {
            return userDto.UserId;
        }

        public static async Task<List<UserDto>> ToDtosAsync(List<string> userIds, IUserService userService, IAuthService authService)
        {
            List<UserDto> userDtos = new();
            foreach (string userId in userIds)
            {
                userDtos.Add(await ToDtoAsync(userId, userService, authService));
            }
            return userDtos;
        }

        public static List<string> ToUserIds(List<UserDto> userDtos)
        {
            List<string> userIds = new();
            foreach (UserDto userDto in userDtos)
            {
                userIds.Add(userDto.UserId);
            }
            return userIds;
        }
    }
}
