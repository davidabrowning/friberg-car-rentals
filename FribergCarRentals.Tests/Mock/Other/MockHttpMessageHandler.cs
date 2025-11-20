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
        public string? ExpectedPath { get; set; }
        public Object? ResponseObject { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri == null)
            {
                throw new InvalidOperationException("No path URI given.");
            }

            string actualPath = request.RequestUri.PathAndQuery;
            if (actualPath != ExpectedPath)
            {
                throw new InvalidOperationException($"Incorrect path. Expected path: {ExpectedPath}. Actual path: {actualPath}.");
            }

            HttpResponseMessage httpResponseMessage = new(HttpStatusCode);
            if (ResponseObject != null)
            {
                string responseObjectAsJson = JsonSerializer.Serialize(ResponseObject);
                httpResponseMessage.Content = new StringContent(responseObjectAsJson);
            }
            return Task.FromResult(httpResponseMessage);
        }
    }
}
