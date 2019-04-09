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

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private IIdentityUnitOfWork Database { get; set; }
        private IMapper mapper { get; set; }

        /// <summary>
        /// Dependency Injection to database repositories.
        /// </summary>
        /// <param name="unitOfWork"> Point to context of dataBase </param>
        public UserService(IIdentityUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
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

                Database.People.Update(person.Id, person);
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
                Database.People.Update(person.Id, person);
            }
            else if ((roleName == "Admin") || (userRole == "Admin"))
            {
                return new IdentityOperation(false, "Only the one administrator have to be in database", "userRole");
            }
            else if (roleName == userRole)
            {
            }
            else
            {
                return new IdentityOperation(false, "Current role cannot find in database", "roleName");
            }

            await Database.SaveAsync();
            return new IdentityOperation(true, "Role has been changed", "");
        }

        public async Task<IEnumerable<UserDTO>> GetUsersInRoleAsync(string roleName)
        {
            var users = await Database.Users.GetUsersInRoleAsync(roleName);
            var persons = Database.People.Find(p => p.Role == roleName);
            var userDTOs = mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UserDTO>>(users);

            foreach (var person in persons)
            {
                foreach (var userDTO in userDTOs)
                {
                    if (userDTO.Id == person.UserId)
                    {
                        if (person.TeamId != null)
                            userDTO.TeamName = (await Database.Teams.GetByIdAsync(person.TeamId.Value)).TeamName;
                        userDTO.Role = roleName;
                    }
                }
            }

            return userDTOs;
        }

        public async Task<UserDTO> GetUserByNameAsync(string userName)
        {
            var user = await Database.Users.FindByNameAsync(userName);

            if (user == null)
                throw new UserNotFoundException("User with this username has not found");

            return await GetUser(user);
        }

        public async Task<UserDTO> GetUserByIdAsync(string userId)
        {
            var user = await Database.Users.FindByIdAsync(userId);

            if (user == null)
                throw new UserNotFoundException("User with this id has not found");

            return await GetUser(user);
        }

        private async Task<UserDTO> GetUser(ApplicationUser user)
        {
            var role = (await Database.Users.GetRolesAsync(user)).FirstOrDefault();
            var userDTO = mapper.Map<ApplicationUser, UserDTO>(user);
            userDTO.Role = role;

            var person = await Database.People.GetSingleAsync(p => p.UserId == user.Id);

            if (person != null)
                if (person.TeamId != null)
                    userDTO.TeamName = (await Database.Teams.GetByIdAsync(person.TeamId.Value)).TeamName;

            return userDTO;
        }


        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
