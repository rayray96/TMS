using BLL.DTO;
using BLL.Exceptions;
using BLL.Infrastructure;
using BLL.Services;
using DAL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace BLL.Tests.ServicesTests
{
    [TestClass]
    public class UserServiceTests
    {

        #region CreateUserAsync

        [TestMethod]
        public async Task CreateUserAsyncMethod_WithoutFirstUser_ShouldBeReturnedSuccessfulIdentityOperation()
        {
            UserDTO user = new UserDTO
            {
                Email = "josh@test.com",
                FName = "Josh",
                LName = "Collins",
                Password = "qwerty123",
                Role = "Worker",
                UserName = "J."
            };

            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            IIdentityUnitOfWork uow = mock.Object;
            UserService service = new UserService(uow);

            IList<ApplicationUser> applicationUsers = new List<ApplicationUser>();
            var expected = new IdentityOperation(true, "Sign up is success", "");

            mock.Setup(x => x.Users.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);
            mock.Setup(x => x.Users.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).Returns(Task.FromResult(new IdentityResult()));
            mock.Setup(x => x.Users.GetUsersInRoleAsync("Admin")).Returns(Task.FromResult(applicationUsers));


            var actual = await service.CreateUserAsync(user);


            Assert.AreSame(expected.Message, actual.Message);
            Assert.AreSame(expected.Property, actual.Property);
            Assert.AreEqual(expected.Succeeded, actual.Succeeded);
        }

        [TestMethod]
        public async Task CreateUserAsyncMethod_WithFirstUser_ShouldBeReturnedSuccessfulIdentityOperation()
        {
            UserDTO user = new UserDTO
            {
                Email = "josh@test.com",
                FName = "Josh",
                LName = "Collins",
                Password = "qwerty123",
                Role = "Worker",
                UserName = "J."
            };

            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            IIdentityUnitOfWork uow = mock.Object;
            UserService service = new UserService(uow);

            IList<ApplicationUser> applicationUsers = new List<ApplicationUser>();
            applicationUsers.Insert(0, new ApplicationUser());
            var expected = new IdentityOperation(true, "Sign up is success", "");

            mock.Setup(x => x.Users.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);
            mock.Setup(x => x.Users.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).Returns(Task.FromResult(new IdentityResult()));
            mock.Setup(x => x.Users.GetUsersInRoleAsync("Admin")).Returns(Task.FromResult(applicationUsers));
            mock.Setup(x => x.People.Create(It.IsAny<Person>()));


            var actual = await service.CreateUserAsync(user);


            Assert.AreSame(expected.Message, actual.Message);
            Assert.AreSame(expected.Property, actual.Property);
            Assert.AreEqual(expected.Succeeded, actual.Succeeded);
        }

        [TestMethod]
        public async Task CreateUserAsyncMethod_UserAlreadyExists_ShouldBeReturnedFailedIdentityOperation()
        {
            UserDTO user = new UserDTO
            {
                Email = "josh@test.com",
                FName = "Josh",
                LName = "Collins",
                Password = "qwerty123",
                Role = "Worker",
                UserName = "J."
            };

            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            IIdentityUnitOfWork uow = mock.Object;
            UserService service = new UserService(uow);

            IList<ApplicationUser> applicationUsers = new List<ApplicationUser>(1);
            var expected = new IdentityOperation(false, "User with this email is already exist", "Email");

            mock.Setup(x => x.Users.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());


            var actual = await service.CreateUserAsync(user);


            Assert.AreSame(expected.Message, actual.Message);
            Assert.AreSame(expected.Property, actual.Property);
            Assert.AreEqual(expected.Succeeded, actual.Succeeded);
        }

        [TestMethod]
        public async Task CreateUserAsyncMethod_CannotCreateUser_ShouldBeReturnedFailedIdentityOperation()
        {
            UserDTO user = new UserDTO
            {
                Email = "josh@test.com",
                FName = "Josh",
                LName = "Collins",
                Password = "qwerty123",
                Role = "Worker",
                UserName = "J."
            };

            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            IIdentityUnitOfWork uow = mock.Object;
            UserService service = new UserService(uow);

            IList<ApplicationUser> applicationUsers = new List<ApplicationUser>();
            applicationUsers.Insert(0, new ApplicationUser());

            mock.Setup(x => x.Users.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);
            mock.Setup(x => x.Users.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).Returns(Task.FromResult(new IdentityResult()));
            mock.Setup(x => x.Users.GetUsersInRoleAsync("Admin")).Returns(Task.FromResult(applicationUsers));
            // If we already have duplicate of username in the database, for example.
            mock.Setup(x => x.People.Create(It.IsAny<Person>())).Throws(new ArgumentException());


            var actual = await service.CreateUserAsync(user);


            Assert.AreEqual(false, actual.Succeeded);
        }

        #endregion

        #region UpdateUserRoleAsync

        [TestMethod]
        public async Task UpdateUserRoleAsyncMethod_UserIdNotExists_ShouldBeReturnedFailedIdentityOperation()
        {
            string userId = "1a", rolename = "Worker";

            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            IIdentityUnitOfWork uow = mock.Object;
            UserService service = new UserService(uow);

            var expected = new IdentityOperation(false, "User with this Id is not found", "userId");

            mock.Setup(x => x.Users.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);


            var actual = await service.UpdateUserRoleAsync(userId, rolename);


            Assert.AreSame(expected.Message, actual.Message);
            Assert.AreSame(expected.Property, actual.Property);
            Assert.AreEqual(expected.Succeeded, actual.Succeeded);
        }

        [TestMethod]
        public async Task UpdateUserRoleAsyncMethod_TwoManagersInTeam_ShouldBeReturnedFailedIdentityOperation()
        {
            string userId = "1a", rolename = "Manager";
            IList<string> roles = new List<string>();
            roles.Add("Worker");

            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            IIdentityUnitOfWork uow = mock.Object;
            UserService service = new UserService(uow);

            var expected = new IdentityOperation(false, "In one team cannot be two manager", "userRole");

            mock.Setup(x => x.Users.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            mock.Setup(x => x.Users.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(roles);
            mock.Setup(x => x.People.GetSingleAsync(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(new Person { TeamId = 1 });


            var actual = await service.UpdateUserRoleAsync(userId, rolename);


            Assert.AreSame(expected.Message, actual.Message);
            Assert.AreSame(expected.Property, actual.Property);
            Assert.AreEqual(expected.Succeeded, actual.Succeeded);
        }

        [TestMethod]
        public async Task UpdateUserRoleAsyncMethod_FromWorkerToManager_ShouldBeReturnedSuccessfulIdentityOperation()
        {
            string userId = "1a", rolename = "Manager";
            IList<string> roles = new List<string>();
            roles.Add("Worker");

            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            IIdentityUnitOfWork uow = mock.Object;
            UserService service = new UserService(uow);

            var expected = new IdentityOperation(true, "Role has been changed", "");

            mock.Setup(x => x.Users.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            mock.Setup(x => x.Users.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(roles);
            mock.Setup(x => x.People.GetSingleAsync(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(new Person { TeamId = null });


            var actual = await service.UpdateUserRoleAsync(userId, rolename);


            Assert.AreSame(expected.Message, actual.Message);
            Assert.AreSame(expected.Property, actual.Property);
            Assert.AreEqual(expected.Succeeded, actual.Succeeded);
        }

        [TestMethod]
        public async Task UpdateUserRoleAsyncMethod_FromManagerToWorker_ShouldBeReturnedSuccessfulIdentityOperation()
        {
            string userId = "1a", rolename = "Worker";
            IList<string> roles = new List<string>();
            roles.Add("Manager");

            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            IIdentityUnitOfWork uow = mock.Object;
            UserService service = new UserService(uow);

            var expected = new IdentityOperation(true, "Role has been changed", "");

            mock.Setup(x => x.Users.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            mock.Setup(x => x.Users.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(roles);
            mock.Setup(x => x.People.GetSingleAsync(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(new Person { TeamId = null });


            var actual = await service.UpdateUserRoleAsync(userId, rolename);


            Assert.AreSame(expected.Message, actual.Message);
            Assert.AreSame(expected.Property, actual.Property);
            Assert.AreEqual(expected.Succeeded, actual.Succeeded);
        }

        [TestMethod]
        public async Task UpdateUserRoleAsyncMethod_ManagerWithTeamToWorker_ShouldBeReturnedFailedIdentityOperation()
        {
            string userId = "1a", rolename = "Worker";
            IList<string> roles = new List<string>();
            roles.Add("Manager");

            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            IIdentityUnitOfWork uow = mock.Object;
            UserService service = new UserService(uow);

            var expected = new IdentityOperation(false, "Cannot change role, this manager has got a team", "userRole");

            mock.Setup(x => x.Users.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            mock.Setup(x => x.Users.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(roles);
            mock.Setup(x => x.People.GetSingleAsync(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(new Person { TeamId = 1 });


            var actual = await service.UpdateUserRoleAsync(userId, rolename);


            Assert.AreSame(expected.Message, actual.Message);
            Assert.AreSame(expected.Property, actual.Property);
            Assert.AreEqual(expected.Succeeded, actual.Succeeded);
        }

        [TestMethod]
        public async Task UpdateUserRoleAsyncMethod_ToAdmin_ShouldBeReturnedFailedIdentityOperation()
        {
            string userId = "1a", rolename = "Admin";
            IList<string> roles = new List<string>();
            roles.Add("Manager");

            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            IIdentityUnitOfWork uow = mock.Object;
            UserService service = new UserService(uow);

            var expected = new IdentityOperation(false, "Only the one administrator have to be in database", "userRole");

            mock.Setup(x => x.Users.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            mock.Setup(x => x.Users.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(roles);


            var actual = await service.UpdateUserRoleAsync(userId, rolename);


            Assert.AreSame(expected.Message, actual.Message);
            Assert.AreSame(expected.Property, actual.Property);
            Assert.AreEqual(expected.Succeeded, actual.Succeeded);
        }

        [TestMethod]
        public async Task UpdateUserRoleAsyncMethod_FromAdmin_ShouldBeReturnedFailedIdentityOperation()
        {
            string userId = "1a", rolename = "Manager";
            IList<string> roles = new List<string>();
            roles.Add("Admin");

            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            IIdentityUnitOfWork uow = mock.Object;
            UserService service = new UserService(uow);

            var expected = new IdentityOperation(false, "Only the one administrator have to be in database", "userRole");

            mock.Setup(x => x.Users.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            mock.Setup(x => x.Users.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(roles);


            var actual = await service.UpdateUserRoleAsync(userId, rolename);


            Assert.AreSame(expected.Message, actual.Message);
            Assert.AreSame(expected.Property, actual.Property);
            Assert.AreEqual(expected.Succeeded, actual.Succeeded);
        }

        [TestMethod]
        public async Task UpdateUserRoleAsyncMethod_WithoutChanges_ShouldBeReturnedSuccessfulIdentityOperation()
        {
            string userId = "1a", rolename = "Worker";
            IList<string> roles = new List<string>();
            roles.Add("Worker");

            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            IIdentityUnitOfWork uow = mock.Object;
            UserService service = new UserService(uow);

            var expected = new IdentityOperation(true, "Role has been changed", "");

            mock.Setup(x => x.Users.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            mock.Setup(x => x.Users.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(roles);


            var actual = await service.UpdateUserRoleAsync(userId, rolename);


            Assert.AreSame(expected.Message, actual.Message);
            Assert.AreSame(expected.Property, actual.Property);
            Assert.AreEqual(expected.Succeeded, actual.Succeeded);
        }

        [TestMethod]
        public async Task UpdateUserRoleAsyncMethod_RoleNotExists_ShouldBeReturnedFailedIdentityOperation()
        {
            string userId = "1a", rolename = "Director";
            IList<string> roles = new List<string>();
            roles.Add("Worker");

            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            IIdentityUnitOfWork uow = mock.Object;
            UserService service = new UserService(uow);

            var expected = new IdentityOperation(false, "Current role cannot find in database", "roleName");

            mock.Setup(x => x.Users.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            mock.Setup(x => x.Users.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(roles);


            var actual = await service.UpdateUserRoleAsync(userId, rolename);


            Assert.AreSame(expected.Message, actual.Message);
            Assert.AreSame(expected.Property, actual.Property);
            Assert.AreEqual(expected.Succeeded, actual.Succeeded);
        }

        #endregion

        #region GetUsersInRoleAsync

        [TestMethod]
        public async Task GetUsersInRoleAsyncMethod_WorkersExist_ShouldBeReturnedAllWorkers()
        {
            IList<ApplicationUser> users = new List<ApplicationUser>()
            {
                new ApplicationUser() { Id = "1a" },
                new ApplicationUser() { Id = "2b" },
                new ApplicationUser() { Id = "3c" },
            };

            IEnumerable<Person> persons = new List<Person>()
            {
                new Person() { Role = "Worker", TeamId = 1, UserId = "1a" },
                new Person() { Role = "Worker", TeamId = 2, UserId = "2b" },
                new Person() { Role = "Worker", TeamId = 3, UserId = "3c" },
            };

            IList<Team> teams = new List<Team>()
            {
                new Team() { Id = 1, TeamName = "TestTeam1" },
                new Team() { Id = 2, TeamName = "TestTeam2" },
                new Team() { Id = 3, TeamName = "TestTeam3" },
                new Team() { Id = 4, TeamName = "TestTeam4" }
            };

            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            IIdentityUnitOfWork uow = mock.Object;
            UserService service = new UserService(uow);

            var expected = new List<UserDTO>()
            {
                new UserDTO() { Id = "1a", TeamName = "TestTeam1", Role = "Worker" },
                new UserDTO() { Id = "2b", TeamName = "TestTeam2", Role = "Worker" },
                new UserDTO() { Id = "3c", TeamName = "TestTeam3", Role = "Worker" }
            };

            mock.Setup(x => x.Users.GetUsersInRoleAsync("Worker")).ReturnsAsync(users);
            mock.Setup(x => x.People.Find(It.IsAny<Expression<Func<Person, bool>>>())).Returns(persons);
            mock.Setup(x => x.Teams.GetByIdAsync(It.IsAny<int>())).Returns((int Id) =>
            {
                return Task.FromResult((from item in teams
                                        where item.Id == Id
                                        select item).FirstOrDefault());
            });


            var actual = await service.GetUsersInRoleAsync("Worker");


            CollectionAssert.AreEqual(expected.Select(x => x.Id).ToList(), actual.Select(x => x.Id).ToList());
            CollectionAssert.AreEqual(expected.Select(x => x.TeamName).ToList(), actual.Select(x => x.TeamName).ToList());
            CollectionAssert.AreEqual(expected.Select(x => x.Role).ToList(), actual.Select(x => x.Role).ToList());
        }

        #endregion

        #region GetUserByNameAsync

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task GetUserByNameAsyncMethod_NameNotExist_ShouldBeThrownUserNotFoundException()
        {
            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            IIdentityUnitOfWork uow = mock.Object;
            UserService service = new UserService(uow);

            mock.Setup(x => x.Users.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);


            await service.GetUserByNameAsync(It.IsAny<string>());
        }

        [TestMethod]
        public async Task GetUserByNameAsyncMethod_OnlyAdminExists_ShouldBeReturnedUser()
        {
            var expected = new UserDTO { Role = "Admin" };

            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            IIdentityUnitOfWork uow = mock.Object;
            UserService service = new UserService(uow);

            mock.Setup(x => x.Users.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            mock.Setup(x => x.Users.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string>() { "Admin" });
            mock.Setup(x => x.People.GetSingleAsync(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync((Person)null);


            var actual = await service.GetUserByNameAsync(It.IsAny<string>());


            Assert.AreSame(expected.Role, actual.Role);
        }

        [TestMethod]
        public async Task GetUserByNameAsyncMethod_UserInTeam_ShouldBeReturnedUser()
        {
            var expected = new UserDTO { Role = "Worker", TeamName = "TestTeam1" };

            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            IIdentityUnitOfWork uow = mock.Object;
            UserService service = new UserService(uow);

            mock.Setup(x => x.Users.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            mock.Setup(x => x.Users.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string>() { "Worker" });
            mock.Setup(x => x.People.GetSingleAsync(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(new Person() { TeamId = 1 });
            mock.Setup(x => x.Teams.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Team() { TeamName = "TestTeam1" });


            var actual = await service.GetUserByNameAsync(It.IsAny<string>());


            Assert.AreSame(expected.Role, actual.Role);
            Assert.AreSame(expected.TeamName, actual.TeamName);
        }

        #endregion

        #region GetUserByIdAsync

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task GetUserByIdAsyncMethod_NameNotExist_ShouldBeThrownUserNotFoundException()
        {
            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            IIdentityUnitOfWork uow = mock.Object;
            UserService service = new UserService(uow);

            mock.Setup(x => x.Users.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);


            await service.GetUserByIdAsync(It.IsAny<string>());
        }

        [TestMethod]
        public async Task GetUserByIdAsyncMethod_OnlyAdminExists_ShouldBeReturnedUser()
        {
            var expected = new UserDTO { Role = "Admin" };

            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            IIdentityUnitOfWork uow = mock.Object;
            UserService service = new UserService(uow);

            mock.Setup(x => x.Users.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            mock.Setup(x => x.Users.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string>() { "Admin" });
            mock.Setup(x => x.People.GetSingleAsync(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync((Person)null);


            var actual = await service.GetUserByIdAsync(It.IsAny<string>());


            Assert.AreSame(expected.Role, actual.Role);
        }

        [TestMethod]
        public async Task GetUserByIdAsyncMethod_UserInTeam_ShouldBeReturnedUser()
        {
            var expected = new UserDTO { Role = "Manager", TeamName = "TestTeam1" };

            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            IIdentityUnitOfWork uow = mock.Object;
            UserService service = new UserService(uow);

            mock.Setup(x => x.Users.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            mock.Setup(x => x.Users.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string>() { "Manager" });
            mock.Setup(x => x.People.GetSingleAsync(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(new Person() { TeamId = 1 });
            mock.Setup(x => x.Teams.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Team() { TeamName = "TestTeam1" });


            var actual = await service.GetUserByIdAsync(It.IsAny<string>());


            Assert.AreSame(expected.Role, actual.Role);
            Assert.AreSame(expected.TeamName, actual.TeamName);
        }

        #endregion

    }
}
