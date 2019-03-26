using BLL.DTO;
using BLL.Infrastructure;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using BLL.Exceptions;
using AutoMapper;
using BLL.Configurations;

namespace BLL.Services
{
    public class TokenService: ITokenService
    {
        private IIdentityUnitOfWork Database { get; set; }
        private IConfiguration Configuration { get; set; }
        private IMapper mapper { get; set; }

        public TokenService(IIdentityUnitOfWork unitOfWork, IConfiguration configuration)
        {
            Database = unitOfWork;
            Configuration = configuration;
            mapper = MapperConfig.GetMapperResult();
        }
        // Done!
        public async Task<ClaimsIdentity> GetClaimsIdentityAsync(string userName, string password)
        {
            ClaimsIdentity claimsIdentity = null;

            var user = await Database.Users.FindByNameAsync(userName);

            if (user != null)
            {
                var checkPassword = await Database.Users.CheckPasswordAsync(user, password);
                if (checkPassword)
                {
                    var role = await Database.Users.GetRolesAsync(user);
                    if (role != null)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                            new Claim(ClaimsIdentity.DefaultRoleClaimType, role.FirstOrDefault())
                        };

                        claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                                                            ClaimsIdentity.DefaultRoleClaimType);
                    }
                    else
                    {
                        throw new Exception("This user does not have a role"); // TODO: exception.
                    }
                }
                else
                {
                    throw new Exception("This password is not valid");
                }
            }
            else
            {
                throw new Exception("This user does not exist");
            }

            return claimsIdentity;
        }
        // Done!
        public async Task<ClaimsIdentity> GetClaimsIdentityAsync(string userId)
        {
            ClaimsIdentity claimsIdentity = null;

            var user = await Database.Users.FindByIdAsync(userId);

            if (user != null)
            {
                var role = await Database.Users.GetRolesAsync(user);

                if (role != null)
                {
                    var claims = new List<Claim>
                    {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role.FirstOrDefault())
                    };

                    claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                                                        ClaimsIdentity.DefaultRoleClaimType);
                }
                else
                {
                    throw new Exception("This user does not have a role"); // TODO: exception.
                }
            }
            else
            {
                throw new Exception("This user does not exist");
            }

            return claimsIdentity;
        }
        
        public string GenerateToken(IEnumerable<Claim> claims, double lifeTime)
        {
            var now = DateTime.UtcNow;
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Jwt:Key")));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // Creating JWT-token.
            var jwt = new JwtSecurityToken(
                issuer: Configuration.GetValue<string>("Jwt:Issuer"),
                audience: Configuration.GetValue<string>("Jwt:Audience"),
                notBefore: now,
                claims: claims,
                expires: now.Add(TimeSpan.FromMinutes(lifeTime)),
                signingCredentials: signingCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
        // Done!
        public RefreshTokenDTO GetRefreshToken(string refreshToken)
        {
            if (refreshToken != null)
            {
                var result = Database.RefreshTokens.Find(t => t.Token == refreshToken).FirstOrDefault();
                return mapper.Map<RefreshToken, RefreshTokenDTO>(result);
            }
            else
            {
                throw new Exception("Refresh token has not found"); // TODO: exception.
            }
        }
        // Done!
        public void UpdateRefreshToken(RefreshTokenDTO refreshToken)
        {
            var _refreshToken = Database.RefreshTokens.GetById(refreshToken.Id);
            if (_refreshToken != null)
            {
                _refreshToken.Token = Guid.NewGuid().ToString();
                Database.RefreshTokens.Update(_refreshToken);
                Database.Save();
            }
            else
            {
                throw new Exception("Refresh token has not found"); // TODO: Exception.
            }
        }
        // Done!
        public async Task<RefreshTokenDTO> GenerateRefreshTokenAsync(string userName)
        {
            ApplicationUser appUser = await Database.Users.FindByNameAsync(userName);
            if (appUser != null)
            {
                var refreshDbToken = Database.RefreshTokens.Find(n => n.UserId == appUser.Id).FirstOrDefault();
                if (refreshDbToken != null)
                {
                    Database.RefreshTokens.Delete(refreshDbToken.Id);
                    await Database.SaveAsync();
                }

                var newRefreshToken = new RefreshToken
                {
                    UserId = appUser.Id,
                    Token = Guid.NewGuid().ToString(),
                    Issue = DateTime.Now,
                    Expires = DateTime.Now.AddMinutes(10)
                };

                Database.RefreshTokens.Create(newRefreshToken);
                await Database.SaveAsync();

                return mapper.Map<RefreshToken, RefreshTokenDTO>(newRefreshToken);
            }
            else
            {
                throw new Exception("User with this username is not exist"); // TODO: Exception
            }
        }
        
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
