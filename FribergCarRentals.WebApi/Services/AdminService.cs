using FribergCarRentals.Core.Interfaces.Repositories;
using FribergCarRentals.Core.Interfaces.Services;
using FribergCarRentals.Core.Models;

namespace FribergCarRentals.WebApi.Services
{
    public class AdminService : BasicCRUDService<Admin>, IAdminService
    {
        public AdminService(IRepository<Admin> adminRepository) : base(adminRepository)
        {
        }
    }
}
