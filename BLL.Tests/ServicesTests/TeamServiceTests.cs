using BLL.DTO;
using BLL.Exceptions;
using BLL.Services;
using DAL.Interfaces;
using DAL.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BLL.Tests.ServicesTests
{
    [TestClass]
    public class TeamServiceTests
    {
        #region GetAllTeams

        [TestMethod]
        public void GetAllTeamsMethod_TeamsExist_ShouldBeReturnedAllTeams()
        {
            IEnumerable<Team> teams = new List<Team>()
            {
                new Team() { Id=1, TeamName="TestTeam1" },
                new Team() { Id=2, TeamName="TestTeam2" },
                new Team() { Id=3, TeamName="TestTeam3" }
            };

            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();
            IUnitOfWork uow = mock.Object;
            TeamService service = new TeamService(uow);

            var expected = new List<TeamDTO>()
            {
                new TeamDTO() { Id=1, TeamName="TestTeam1" },
                new TeamDTO() { Id=2, TeamName="TestTeam2" },
                new TeamDTO() { Id=3, TeamName="TestTeam3" }
            };
            mock.Setup(x => x.Teams.GetAll()).Returns(teams);


            var actual = service.GetAllTeams();


            CollectionAssert.AreEquivalent(expected.Select(x => x.Id).ToList(), expected.Select(x => x.Id).ToList());
            CollectionAssert.AreEquivalent(expected.Select(x => x.TeamName).ToList(), expected.Select(x => x.TeamName).ToList());
        }

        #endregion

        #region ChangeTeamName

        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void ChangeTeamMethod_InvalidTeamId_ShouldBeThrownTeamNotFoundException()
        {
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();
            IUnitOfWork uow = mock.Object;
            TeamService service = new TeamService(uow);

            mock.Setup(x => x.Teams.GetById(It.IsAny<int>())).Returns((Team)null);

            service.ChangeTeamName(It.IsAny<int>(), It.IsAny<string>());
        }

        [TestMethod]
        public void ChangeTeamNameMethod_TeamExists_ShouldBeSavedNewRole()
        {
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();
            IUnitOfWork uow = mock.Object;
            TeamService service = new TeamService(uow);

            mock.Setup(x => x.Teams.GetById(It.IsAny<int>())).Returns(new Team() { TeamName = "TestTeam1" });


            service.ChangeTeamName(It.IsAny<int>(), "NewTestTeam1");


            mock.Verify(x => x.Save());
        }

        #endregion

        #region GetTeamNameById

        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void GetTeamNameByIdMethod_InvalidTeamId_ShouldBeThrownTeamNotFoundException()
        {
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();
            IUnitOfWork uow = mock.Object;
            TeamService service = new TeamService(uow);

            mock.Setup(x => x.Teams.GetById(It.IsAny<int>())).Returns((Team)null);


            service.GetTeamNameById(It.IsAny<int>());
        }

        [TestMethod]
        public void GetTeamNameByIdMethod_TeamExists_ShouldBeSavedNewRole()
        {
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();
            IUnitOfWork uow = mock.Object;
            TeamService service = new TeamService(uow);

            var expected = "TestTeam1";

            mock.Setup(x => x.Teams.GetById(It.IsAny<int>())).Returns(new Team() { TeamName = "TestTeam1" });


            var actual = service.GetTeamNameById(It.IsAny<int>());


            Assert.AreSame(expected, actual);
        }

        #endregion

        #region CreateTeam

        [TestMethod]
        [ExpectedException(typeof(TeamExistsException))]
        public void CreateTeamMethod_ManagerHasTeam_ShouldBeThrownTeamExistsException()
        {
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();
            IUnitOfWork uow = mock.Object;
            TeamService service = new TeamService(uow);


            service.CreateTeam(new PersonDTO() { TeamId = 1 }, It.IsAny<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(TeamExistsException))]
        public void CreateTeamMethod_TeamExists_ShouldBeThrownTeamExistsException()
        {
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();
            IUnitOfWork uow = mock.Object;
            TeamService service = new TeamService(uow);

            mock.Setup(x => x.Teams.Find(It.IsAny<Expression<Func<Team, bool>>>())).Returns(new List<Team>() { new Team() });

            service.CreateTeam(new PersonDTO(), It.IsAny<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(ManagerNotFoundException))]
        public void CreateTeamMethod_PersonIsNotManager_ShouldBeThrownManagerNotFoundException()
        {
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();
            IUnitOfWork uow = mock.Object;
            TeamService service = new TeamService(uow);

            mock.Setup(x => x.Teams.Find(It.IsAny<Expression<Func<Team, bool>>>())).Returns(new List<Team>());

            service.CreateTeam(new PersonDTO() { Role = "Worker" }, It.IsAny<string>());
        }

        [TestMethod]
        public void CreateTeamMethod_ManagerCreatingTeam_ShouldBeSavedNewTeam()
        {
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();
            IUnitOfWork uow = mock.Object;
            TeamService service = new TeamService(uow);

            mock.Setup(x => x.Teams.Find(It.IsAny<Expression<Func<Team, bool>>>())).Returns(new List<Team>());
            mock.Setup(x => x.People.Update(It.IsAny<int>(), It.IsAny<Person>()));


            service.CreateTeam(new PersonDTO() { Id = 1, Role = "Manager" }, "TestTeam1");


            mock.Verify(x => x.Save());
        }

        #endregion
    }
}
