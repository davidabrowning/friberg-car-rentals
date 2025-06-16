using FribergCarRentals.Data;
using FribergCarRentals.Interfaces;
using FribergCarRentals.Models;

namespace FribergCarRentals.Services
{
    public class AdminService : BasicCRUDService<Admin>, IAdminService
    {
        public AdminService(IRepository<Admin> adminRepository) : base(adminRepository)
        {
        }
    }
}
