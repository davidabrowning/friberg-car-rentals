using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.Core.Interfaces.Other
{
    public interface IDatabaseSeeder
    {
        Task SeedAsync();
    }
}
