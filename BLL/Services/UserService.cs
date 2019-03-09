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

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private IIdentityUnitOfWork Database { get; set; }

        public UserService(IIdentityUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public async Task<IdentityOperation> Create(UserDTO userDTO)
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
                    await Database.Users.AddToRoleAsync(appUser, userDTO.Role);

                    var person = new Person { UserId = appUser.Id, Email = userDTO.Email, Name = userDTO.UserName, Role = userDTO.Role };
                    try
                    {
                        if (userDTO.Role == "Manager")
                        {
                            try
                            {
                                Database.People.CreateTeam(person, userDTO.TeamName);
                            }
                            catch (ArgumentException e)
                            {
                                return new IdentityOperation(false, e.Message, e.ParamName);
                            }
                        }
                        else
                        {
                            Database.People.Create(person);
                        }
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

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDTO)
        {
            ClaimsIdentity id = null;
            var user = await Database.Users.FindByLoginAsync(userDTO.UserName, userDTO.Password);

            if (user != null)
            {
                // создаем один claim
                var claims = new List<Claim>
                {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email)
                };
                // создаем объект ClaimsIdentity
                id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            }

            return id;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
