using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.Core.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateJwtToken(string userId, string username, List<string> roles);
    }
}
