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
        private readonly string _expectedPath;
        private readonly Object? _responseObject;
        private readonly HttpStatusCode _httpStatusCode;

        public MockHttpMessageHandler(string expectedPath, Object? responseObject, HttpStatusCode httpStatusCode)
        {
            _expectedPath = expectedPath;
            _responseObject = responseObject;
            _httpStatusCode = httpStatusCode;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri == null)
            {
                throw new InvalidOperationException("No path URI given.");
            }

            string actualPath = request.RequestUri.PathAndQuery;
            if (actualPath != _expectedPath)
            {
                throw new InvalidOperationException($"Incorrect path. Expected path: {_expectedPath}. Actual path: {actualPath}.");
            }

            HttpResponseMessage httpResponseMessage = new(_httpStatusCode);
            if (_responseObject != null)
            {
                string responseObjectAsJson = JsonSerializer.Serialize(_responseObject);
                httpResponseMessage.Content = new StringContent(responseObjectAsJson);
            }
            return Task.FromResult(httpResponseMessage);
        }
    }
}
