using FribergCarRentals.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.Core.Interfaces.ApiClients
{
    public interface ICarApiClient
    {
        Task<IEnumerable<Car>> GetAllAsync();
    }
}
