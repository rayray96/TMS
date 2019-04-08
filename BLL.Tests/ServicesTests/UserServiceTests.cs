using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Infrastructure;
using BLL.Services;
using BLL.Configurations;
using DAL.UnitOfWork;
using DAL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
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

    }
}
