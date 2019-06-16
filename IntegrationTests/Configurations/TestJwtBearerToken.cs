using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApi.IntegrationTests.Configurations
{
    internal static class TestJwtBearerToken
    {
        public static string UseToken(string userName, string role)
        {
            var claims = new List<Claim>
            {
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
