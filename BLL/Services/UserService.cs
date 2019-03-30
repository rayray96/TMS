using AutoMapper;
using BLL.DTO;
using BLL.Configurations;
using BLL.Infrastructure;
using BLL.Interfaces;
using BLL.Exceptions;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

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
            mapper = MapperConfig.GetIdentityMapperResult();
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

        public async Task<IdentityOperation> UpdateUserRoleAsync(string userId, string roleName)
        {
            var user = await Database.Users.FindByIdAsync(userId);
            if (user == null)
                return new IdentityOperation(false, "User with this Id is not found", "userId");

            var userRole = (await Database.Users.GetRolesAsync(user)).FirstOrDefault();

            if ((roleName == "Manager") && (userRole == "Worker"))
            {
                var person = await Database.People.GetSingleAsync(p => p.UserId == user.Id);

                if (person.TeamId != null)
                    return new IdentityOperation(false, "In one team cannot be two manager", "userRole");
                // Changing role for User.
                await Database.Users.AddToRoleAsync(user, "Manager");
                await Database.Users.RemoveFromRoleAsync(user, "Worker");
                // Changing role for Person.
                person.Role = roleName;
                Database.People.Update(person);
            }
            else if ((roleName == "Worker") && (userRole == "Manager"))
            {
                var person = await Database.People.GetSingleAsync(p => p.UserId == user.Id);

                if (person.TeamId != null)
                    return new IdentityOperation(false, "Cannot change role, this manager has got a team", "userRole");
                // Changing role for User.
                await Database.Users.AddToRoleAsync(user, "Worker");
                await Database.Users.RemoveFromRoleAsync(user, "Manager");
                // Changing role for Person.
                person.Role = roleName;
                Database.People.Update(person);
            }
            else if ((roleName == "Admin") || (userRole == "Admin"))
            {
                return new IdentityOperation(false, "Only the one administrator have to be in database", "userRole");
            }
            else
            {
                return new IdentityOperation(false, "Current role cannot find in database", "roleName");
            }

            await Database.SaveAsync();
            return new IdentityOperation(true, "Role has been changed", "");
        }

        public async Task<IEnumerable<UserDTO>> GetAllWorkersAsync()
        {
            var users = await Database.Users.GetUsersInRoleAsync("Worker");
            var userDTOs = mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UserDTO>>(users);

            foreach (var userDTO in userDTOs)
                userDTO.Role = "Worker";

            return userDTOs;
        }

        public async Task<IEnumerable<UserDTO>> GetAllManagersAsync()
        {
            var users = await Database.Users.GetUsersInRoleAsync("Manager");
            var userDTOs = mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UserDTO>>(users);

            foreach (var userDTO in userDTOs)
                userDTO.Role = "Manager";

            return userDTOs;
        }
        // Fixed bugs with role.
        public async Task<UserDTO> GetUserAsync(string userName)
        {
            var user = await Database.Users.FindByNameAsync(userName);

            if (user == null)
                throw new UserNotFoundException("User with this username has not found");

            var role = await Database.Users.GetRolesAsync(user);
            var userDTO = mapper.Map<ApplicationUser, UserDTO>(user);
            userDTO.Role = role.FirstOrDefault();

            return userDTO;
        }

        public async Task<UserDTO> GetUserByIdAsync(string userId)
        {
            var user = await Database.Users.FindByIdAsync(userId);

            if (user == null)
                throw new UserNotFoundException("User with this id has not found");

            var role = await Database.Users.GetRolesAsync(user);
            var userDTO = mapper.Map<ApplicationUser, UserDTO>(user);
            userDTO.Role = role.FirstOrDefault();

            return userDTO;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
