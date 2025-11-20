using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.Tests.Mock.Other
{
    public class MockHttpClient : HttpClient
    {
        public MockHttpClient(HttpMessageHandler httpMessageHandler)
        {
            this.BaseAddress = new Uri("https://localhost:7175");
        }
    }
}
