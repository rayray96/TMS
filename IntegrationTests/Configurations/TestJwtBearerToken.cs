using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Net.Http.Headers;

namespace WebApi.IntegrationTests.Configurations
{
    internal static class TestJwtBearerToken
    {
        private const string scheme = "Bearer";

        public static void UseToken(this HttpClient httpClient, string userName, string role, string id)
        {
            var jwt = CreateToken(userName, role, id);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, jwt);
        }

        private static string CreateToken(string userName, string role, string id)
        {
            var claims = new List<Claim>
            {
                new Claim("UserId", id),
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                                                ClaimsIdentity.DefaultRoleClaimType);
            var now = DateTime.UtcNow;
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(("BE545654326135461713134126EF")));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // Creating JWT-token.
            var jwt = new JwtSecurityToken(
                issuer: "https://localhost:44360/",
                audience: "https://localhost:44360/",
                notBefore: now,
                claims: claimsIdentity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(60)),
                signingCredentials: signingCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }
}
