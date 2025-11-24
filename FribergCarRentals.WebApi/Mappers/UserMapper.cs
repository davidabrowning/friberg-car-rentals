using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.WebApi.Mappers
{
    public static class UserMapper
    {
        public static async Task<UserDto> ToDtoAsync(string userId, IUserService userService, IAuthService authService)
        {
            UserDto userDto = new()
            {
                UserId = userId,
                Username = await userService.GetUsernameByUserIdAsync(userId) ?? null,
                AuthRoles = await authService.GetRolesAsync(userId),
                AdminDto = AdminMapper.ToDto(await userService.GetAdminByUserIdAsync(userId)) ?? null,
                CustomerDto = CustomerMapper.ToDto(await userService.GetCustomerByUserIdAsync(userId)) ?? null,
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
