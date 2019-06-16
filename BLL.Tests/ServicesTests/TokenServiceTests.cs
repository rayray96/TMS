using BLL.Exceptions;
using BLL.Services;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BLL.Tests.ServicesTests
{
    [TestClass]
    public class TokenServiceTests
    {

        #region GetClaimsIdentityAsync (with two arguments)

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task GetClaimsIdentityAsyncTwoArgMethod_InvalidUserName_ShouldBeThrownUserNotFoundException()
        {
            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            Mock<IConfiguration> conf = new Mock<IConfiguration>();
            TokenService service = new TokenService(mock.Object, conf.Object);

            mock.Setup(x => x.Users.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);


            await service.GetClaimsIdentityAsync(It.IsAny<string>(), It.IsAny<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPasswordException))]
        public async Task GetClaimsIdentityAsyncTwoArgMethod_InvalidPassword_ShouldBeThrownInvalidPasswordException()
        {
            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            Mock<IConfiguration> conf = new Mock<IConfiguration>();
            TokenService service = new TokenService(mock.Object, conf.Object);

            mock.Setup(x => x.Users.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            mock.Setup(x => x.Users.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(false);


            await service.GetClaimsIdentityAsync(It.IsAny<string>(), It.IsAny<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(RoleException))]
        public async Task GetClaimsIdentityAsyncTwoArgMethod_InvalidRole_ShouldBeThrownRoleException()
        {
            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            Mock<IConfiguration> conf = new Mock<IConfiguration>();
            TokenService service = new TokenService(mock.Object, conf.Object);

            mock.Setup(x => x.Users.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            mock.Setup(x => x.Users.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(true);
            mock.Setup(x => x.Users.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string>());


            await service.GetClaimsIdentityAsync(It.IsAny<string>(), It.IsAny<string>());
        }

        [TestMethod]
        public async Task GetClaimsIdentityAsyncTwoArgMethod_GettingClaimsIdentity_ShouldBeReturnedClaimsIdentity()
        {
            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            Mock<IConfiguration> conf = new Mock<IConfiguration>();
            TokenService service = new TokenService(mock.Object, conf.Object);

            mock.Setup(x => x.Users.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser() { UserName = "Joe" });
            mock.Setup(x => x.Users.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(true);
            mock.Setup(x => x.Users.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string>() { "Worker" });


            var actual = await service.GetClaimsIdentityAsync(It.IsAny<string>(), It.IsAny<string>());


            Assert.IsNotNull(actual);
        }

        #endregion

        #region GetClaimsIdentityAsync (with one argument)

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task GetClaimsIdentityAsyncOneArgMethod_InvalidUserId_ShouldBeThrownUserNotFoundException()
        {
            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            Mock<IConfiguration> conf = new Mock<IConfiguration>();
            TokenService service = new TokenService(mock.Object, conf.Object);

            mock.Setup(x => x.Users.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);


            await service.GetClaimsIdentityAsync(It.IsAny<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(RoleException))]
        public async Task GetClaimsIdentityOneArgAsyncMethod_InvalidRole_ShouldBeThrownRoleException()
        {
            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            Mock<IConfiguration> conf = new Mock<IConfiguration>();
            TokenService service = new TokenService(mock.Object, conf.Object);

            mock.Setup(x => x.Users.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            mock.Setup(x => x.Users.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync((IList<string>)null);


            await service.GetClaimsIdentityAsync(It.IsAny<string>());
        }

        [TestMethod]
        public async Task GetClaimsIdentityOneArgAsyncMethod_GettingClaimsIdentity_ShouldBeReturnedClaimsIdentity()
        {
            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            Mock<IConfiguration> conf = new Mock<IConfiguration>();
            TokenService service = new TokenService(mock.Object, conf.Object);

            mock.Setup(x => x.Users.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser() { UserName = "Joe" });
            mock.Setup(x => x.Users.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string>() { "Worker" });


            var actual = await service.GetClaimsIdentityAsync(It.IsAny<string>());


            Assert.IsNotNull(actual);
        }

        #endregion

        #region GenerateRefreshTokenAsync

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public async Task GenerateRefreshTokenAsyncMethod_InvalidUserName_ShouldBeThrownUserNotFoundException()
        {
            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            Mock<IConfiguration> conf = new Mock<IConfiguration>();
            TokenService service = new TokenService(mock.Object, conf.Object);

            mock.Setup(x => x.Users.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);


            await service.GenerateRefreshTokenAsync(It.IsAny<string>());
        }

        [TestMethod]
        public async Task GenerateRefreshTokenAsyncMethod_GeneratingRefreshToken_ShouldBeSavedRefreshToken()
        {
            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            Mock<IConfiguration> conf = new Mock<IConfiguration>();
            TokenService service = new TokenService(mock.Object, conf.Object);

            mock.Setup(x => x.Users.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser() { Id = "1a", UserName = "Joe" });
            mock.Setup(x => x.RefreshTokens.Find(It.IsAny<Expression<Func<RefreshToken, bool>>>())).Returns(new List<RefreshToken>() { new RefreshToken() });
            mock.Setup(x => x.RefreshTokens.Delete(It.IsAny<int>()));
            mock.Setup(x => x.RefreshTokens.Create(It.IsAny<RefreshToken>()));


            var actual = await service.GenerateRefreshTokenAsync(It.IsAny<string>());


            mock.Verify(x => x.SaveAsync());
        }

        [TestMethod]
        public async Task GenerateRefreshTokenAsyncMethod_GeneratingRefreshToken_ShouldBeReturnedRefreshToken()
        {
            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            Mock<IConfiguration> conf = new Mock<IConfiguration>();
            TokenService service = new TokenService(mock.Object, conf.Object);

            mock.Setup(x => x.Users.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser() { Id = "1a", UserName = "Joe" });
            mock.Setup(x => x.RefreshTokens.Find(It.IsAny<Expression<Func<RefreshToken, bool>>>())).Returns(new List<RefreshToken>() { new RefreshToken() });
            mock.Setup(x => x.RefreshTokens.Delete(It.IsAny<int>()));
            mock.Setup(x => x.RefreshTokens.Create(It.IsAny<RefreshToken>()));


            var actual = await service.GenerateRefreshTokenAsync(It.IsAny<string>());


            Assert.IsNotNull(actual);
        }

        #endregion

        #region GetRefreshToken

        [TestMethod]
        [ExpectedException(typeof(RefreshTokenNotFoundException))]
        public void GetRefreshTokenAsyncMethod_RefreshTokenIsNull_ShouldBeThrownRefreshTokenNotFoundException()
        {
            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            Mock<IConfiguration> conf = new Mock<IConfiguration>();
            TokenService service = new TokenService(mock.Object, conf.Object);


            service.GetRefreshToken(It.IsAny<string>());
        }

        #endregion

        #region UpdateRefreshToken

        [TestMethod]
        [ExpectedException(typeof(RefreshTokenNotFoundException))]
        public void UpdateRefreshTokenMethod_RefreshTokenIsNull_ShouldBeThrownRefreshTokenNotFoundException()
        {
            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            Mock<IConfiguration> conf = new Mock<IConfiguration>();
            TokenService service = new TokenService(mock.Object, conf.Object);

            mock.Setup(x => x.RefreshTokens.GetById(It.IsAny<int>())).Returns((RefreshToken)null);


            service.UpdateRefreshToken(new RefreshTokenDTO());
        }

        [TestMethod]
        public void UpdateRefreshTokenMethod_UpdatingRefreshToken_ShouldBeSavedNewRefreshToken()
        {
            Mock<IIdentityUnitOfWork> mock = new Mock<IIdentityUnitOfWork>();
            Mock<IConfiguration> conf = new Mock<IConfiguration>();
            TokenService service = new TokenService(mock.Object, conf.Object);

            mock.Setup(x => x.RefreshTokens.GetById(It.IsAny<int>())).Returns(new RefreshToken());
            mock.Setup(x => x.RefreshTokens.Update(It.IsAny<int>(), It.IsAny<RefreshToken>()));


            service.UpdateRefreshToken(new RefreshTokenDTO());


            mock.Verify(x => x.Save());
        }

        #endregion

    }
}
