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
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using BLL.Exceptions;
using AutoMapper;
using BLL.Configurations;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private IIdentityUnitOfWork Database { get; set; }
        private IConfiguration Configuration { get; set; }
        private IMapper mapper { get; set; }

        public UserService(IIdentityUnitOfWork unitOfWork, IConfiguration configuration)
        {
            Database = unitOfWork;
            Configuration = configuration;
            mapper = MapperConfig.GetMapperResult();
        }

        public async Task<IdentityOperation> CreateAsync(UserDTO userDTO)
        {
            ApplicationUser appUser = await Database.Users.FindByEmailAsync(userDTO.Email);

            if (appUser == null)
            {
                appUser = new ApplicationUser { Email = userDTO.Email, UserName = userDTO.UserName };

                var result = await Database.Users.CreateAsync(appUser, userDTO.Password);

                if (result.Errors.Count() > 0)
                    return new IdentityOperation(false, result.Errors.FirstOrDefault().Code + " " +
                        result.Errors.FirstOrDefault().Description, "");

                var admins = await Database.Users.GetUsersInRoleAsync("Admin");
                if (admins.Count == 0)
                {
                    await Database.Users.AddToRoleAsync(appUser, "Admin");
                }
                else
                {
                    await Database.Users.AddToRoleAsync(appUser, "Worker");

                    var person = new Person { UserId = appUser.Id, Email = userDTO.Email, Name = userDTO.UserName, Role = "Worker" };
                    try
                    {
                        Database.People.Create(person);// отделить?
                    }
                    catch (ArgumentException e)
                    {
                        return new IdentityOperation(false, e.Message, e.ParamName);
                    }
                }
                await Database.SaveAsync();

                return new IdentityOperation(true, "Sign up is success", "");
            }
            else
            {
                return new IdentityOperation(false, "User with this email is already exist", "Email");
            }
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

                    var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role.FirstOrDefault())
                };

                    claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                                                        ClaimsIdentity.DefaultRoleClaimType);
                }
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

                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role.FirstOrDefault())
                };

                claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                                                    ClaimsIdentity.DefaultRoleClaimType);

            }

            return claimsIdentity;
        }

        public string GenerateToken(IEnumerable<Claim> claims, int minutesLife)
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
                expires: now.Add(TimeSpan.FromMinutes(minutesLife)),//(Configuration.GetValue<int>("Jwt:LifeTime"))),
                signingCredentials: signingCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public async Task<IdentityOperation> SetRefreshToken(string userName, string refreshToken)
        {
            ApplicationUser appUser = await Database.Users.FindByNameAsync(userName);
            if (appUser != null)
            {
                if (refreshToken == null)
                    throw new ArgumentNullException("Jwt token is null");// TODO: Add exception for this situation.

                RefreshToken _refreshToken = new RefreshToken { UserId = appUser.Id, Refreshtoken = refreshToken };

                Database.RefreshTokens.Create(_refreshToken);

                //if (result.Errors.Count() > 0)
                //    return new IdentityOperation(false, result.Errors.FirstOrDefault().Code + " " +
                //        result.Errors.FirstOrDefault().Description, "");

                await Database.SaveAsync();

                return new IdentityOperation(true, "Sign up is success", "");
            }
            else
            {
                return new IdentityOperation(false, "User with this username is not exist", "userName");
            }
        }

        public void UpdateToken(RefreshTokenDTO refreshToken)
        {

        }

        public RefreshTokenDTO GetRefreshToken(string refreshToken)
        {
            //ApplicationUser appUser = await Database.Users.FindByNameAsync(userName);
            if (refreshToken != null)
            {
                //if (refreshToken == null)
                //    throw new ArgumentNullException("Jwt token is null");// TODO: Add exception for this situation.

                var result = Database.RefreshTokens.Find(t => t.Refreshtoken == refreshToken).FirstOrDefault();

                //if (result.Errors.Count() > 0)
                //    return new IdentityOperation(false, result.Errors.FirstOrDefault().Code + " " +
                //        result.Errors.FirstOrDefault().Description, "");

                //await Database.SaveAsync();

                //return new IdentityOperation(true, "Sign up is success", "");
                return mapper.Map<RefreshToken, RefreshTokenDTO>(result);
            }
            else
            {
                throw new PersonNotFoundException("Refresh token with this was not found");
            }
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
