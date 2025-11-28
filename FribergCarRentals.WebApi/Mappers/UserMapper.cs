using FribergCarRentals.Services.ApplicationModels;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.WebApi.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToDtoAsync(UserInfoModel userInfoModel)
        {
            AdminDto adminDto = new();
            if (userInfoModel.Admin != null)
                adminDto = AdminMapper.ToDto(userInfoModel.Admin);

            CustomerDto customerDto = new();
            if (userInfoModel.Customer != null)
                customerDto = CustomerMapper.ToDto(userInfoModel.Customer);

            UserDto userDto = new()
            {
                UserId = userInfoModel.UserId,
                Username = userInfoModel.Username,
                AuthRoles = userInfoModel.AuthRoles,
                AdminDto = adminDto,
                CustomerDto = customerDto,
            };
            return userDto;
        }

        public static List<UserDto> ToDtosAsync(List<UserInfoModel> userInfoModels)
        {
            List<UserDto> userDtos = new();
            foreach (UserInfoModel userInfoModel in userInfoModels)
            {
                userDtos.Add(ToDtoAsync(userInfoModel));
            }
            return userDtos;
        }
    }
}
