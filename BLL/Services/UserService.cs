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
        
        public async Task<IdentityOperation> CreateUserAsync(UserDTO userDTO)
        {
            ApplicationUser appUser = await Database.Users.FindByEmailAsync(userDTO.Email);

            if (appUser == null)
            {
                appUser = new ApplicationUser
                {
                    Email = userDTO.Email,
                    UserName = userDTO.UserName,
                    FName = userDTO.FName,
                    LName = userDTO.LName
                };

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

                    var person = new Person
                    {
                        UserId = appUser.Id,
                        Email = userDTO.Email,
                        FName = userDTO.FName,
                        LName = userDTO.LName,
                        UserName = userDTO.UserName,
                        Role = "Worker"
                    };
                    try
                    {
                        Database.People.Create(person);
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

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
