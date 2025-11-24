using FribergCarRentals.Core.Models;
using FribergCarRentals.WebApi.Dtos;

namespace FribergCarRentals.WebApi.Mappers
{
    public static class AdminMapper
    {
        public static AdminDto ToDto(Admin admin)
        {
            AdminDto adminDto = new()
            {
                Id = admin.Id,
                UserId = admin.UserId
            };
            return adminDto;
        }

        public static Admin ToNewModelWIthoutId(AdminDto adminDto)
        {
            Admin admin = new()
            {
                UserId = adminDto.UserId
            };
            return admin;
        }

        public static void UpdateModel(Admin admin, AdminDto adminDto)
        {
            admin.UserId = adminDto.UserId;
        }
    }
}
