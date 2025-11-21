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

        public static Admin ToModel(AdminDto adminDto)
        {
            Admin admin = new()
            {
                Id = adminDto.Id,
                UserId = adminDto.UserId
            };
            return admin;
        }
    }
}
