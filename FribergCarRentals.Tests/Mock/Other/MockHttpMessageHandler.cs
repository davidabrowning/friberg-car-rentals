using System.Net;
using System.Text.Json;

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
