using FribergCarRentals.WebApi.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FribergCarRentals.Mvc.Session
{
    public class JwtReader
    {
        public static string? GetUserId(string token)
        {
            ClaimsIdentity? claimsIdentity = GetClaimsIdentity(token);
            if (claimsIdentity == null)
                return null;
            string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            return userId;
        }

        public static string? GetUsername(string token)
        {
            ClaimsIdentity? claimsIdentity = GetClaimsIdentity(token);
            if (claimsIdentity == null)
                return null;
            string username = claimsIdentity.FindFirst(ClaimTypes.Name).Value;
            return username;
        }

        public static IEnumerable<string> GetRoles(string token)
        {
            ClaimsIdentity? claimsIdentity = GetClaimsIdentity(token);
            if (claimsIdentity == null)
                return null;
            IEnumerable<string> roles = claimsIdentity.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
            return roles;
        }

        private static ClaimsIdentity? GetClaimsIdentity(string token)
        {
            JwtSecurityTokenHandler handler = new();
            JwtSecurityToken? jwtSecurityToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtSecurityToken == null)
                return null;

            ClaimsIdentity claimsIdentity = new(jwtSecurityToken.Claims, "jwt");
            return claimsIdentity;
        }
    }
}
