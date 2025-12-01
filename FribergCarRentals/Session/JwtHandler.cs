using System.Net.Http.Headers;

namespace FribergCarRentals.Mvc.Session
{
    public class JwtHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string? token = _httpContextAccessor.HttpContext?.Request.Cookies["jwt"];

            if (!(token == null || token == string.Empty))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
