using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FribergCarRentals.Tests.Mock.Other
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        public string? RequestPath { get; set; }
        public HttpMethod? RequestMethod { get; set; }
        public Object? ResponseObject { get; set; } = new Object();

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestPath = request.RequestUri?.PathAndQuery;
            RequestMethod = request.Method;

            HttpResponseMessage httpResponseMessage = new(HttpStatusCode.OK);
            string responseObjectAsJson = JsonSerializer.Serialize(ResponseObject);
            httpResponseMessage.Content = new StringContent(responseObjectAsJson);
            return Task.FromResult(httpResponseMessage);
        }
    }
}
