using AutoMapper;
using BLL.Configurations;
using BLL.Configurations.FactoryMethod;
using BLL.Interfaces;
using BLL.Exceptions;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace BLL.Services
{
    public class TokenService : ITokenService
    {
        private IIdentityUnitOfWork Database { get; set; }
        private IConfiguration Configuration { get; set; }
        private IMapper mapper { get; set; }

        public TokenService(IIdentityUnitOfWork unitOfWork, IConfiguration configuration)
        {
            Database = unitOfWork;
            Configuration = configuration;

            // Using Factory Method.
            MapperCreator creator = new IdentityCreator();
            IWrappedMapper wrappedMapper = creator.FactoryMethod();
            mapper = wrappedMapper.CreateMapping();
        }

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
                    if (role.Count != 0)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim("UserId", user.Id),
                            new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                            new Claim(ClaimsIdentity.DefaultRoleClaimType, role.FirstOrDefault())
                        };

                        claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                                                            ClaimsIdentity.DefaultRoleClaimType);
                    }
                    else
                    {
                        throw new RoleException($"This user \"{user.Id}\" does not have a role");
                    }
                }
                else
                {
                    throw new InvalidPasswordException($"This password \"{password}\" is not valid");
                }
            }
            else
            {
                throw new UserNotFoundException($"This user \"{userName}\" does not exist");
            }

            return claimsIdentity;
        }

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
                        new Claim("UserId", user.Id),
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, role.FirstOrDefault())
                    };

                    claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                                                        ClaimsIdentity.DefaultRoleClaimType);
                }
                else
                {
                    throw new RoleException($"This user \"{userId}\" does not have a role");
                }
            }
            else
            {
                throw new UserNotFoundException($"This user \"{userId}\" does not exist");
            }

            return claimsIdentity;
        }

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
                throw new UserNotFoundException($"User with this username \"{userName}\" is not exist");
            }
        }

        public RefreshTokenDTO GetRefreshToken(string refreshToken)
        {
            if (refreshToken != null)
            {
                var result = Database.RefreshTokens.Find(t => t.Token == refreshToken).FirstOrDefault();
                return mapper.Map<RefreshToken, RefreshTokenDTO>(result);
            }
            else
            {
                throw new RefreshTokenNotFoundException($"Refresh token \"{refreshToken}\" has not found");
            }
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

        public void UpdateRefreshToken(RefreshTokenDTO refreshToken)
        {
            var _refreshToken = Database.RefreshTokens.GetById(refreshToken.Id);
            if (_refreshToken != null)
            {
                _refreshToken.Token = Guid.NewGuid().ToString();
                Database.RefreshTokens.Update(_refreshToken.Id, _refreshToken);
                Database.Save();
            }
            else
            {
                throw new RefreshTokenNotFoundException($"Refresh token \"{refreshToken}\" has not found");
            }
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
