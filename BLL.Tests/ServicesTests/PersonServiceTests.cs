using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Services;
using DAL.Interfaces;
using DAL.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BLL.Tests.ServicesTests
{
    [TestClass]
    public class PersonServiceTests
    {

        #region DeletePersonFromTeam

        [TestMethod]
        [ExpectedException(typeof(PersonNotFoundException))]
        public void DeletePersonFromTeamMethod_InvalidPersonId_ShouldBeThrownPersonNotFoundException()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            PersonService service = new PersonService(uow.Object, emailService.Object);

            uow.Setup(x => x.People.GetById(It.IsAny<int>())).Returns((Person)null);


            service.DeletePersonFromTeam(It.IsAny<int>());
        }

        [TestMethod]
        public void DeletePersonFromTeamMethod_DeletingTask_ShouldBeDeletedTask()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            PersonService service = new PersonService(uow.Object, emailService.Object);

            uow.Setup(x => x.People.GetById(It.IsAny<int>())).Returns(new Person());
            uow.Setup(x => x.Tasks.Find(It.IsAny<Expression<Func<TaskInfo, bool>>>())).Returns(new List<TaskInfo>() { new TaskInfo() });
            uow.Setup(x => x.Tasks.Update(It.IsAny<int>(), It.IsAny<TaskInfo>()));
            uow.Setup(x => x.People.Update(It.IsAny<int>(), It.IsAny<Person>()));


            service.DeletePersonFromTeam(It.IsAny<int>());


            uow.Verify(x => x.Save());
        }

        #endregion

        #region AddPersonsToTeam

        [TestMethod]
        [ExpectedException(typeof(PersonNotFoundException))]
        public void AddPersonsToTeamMethod_NoPersonsToAdding_ShouldBeThrownPersonNotFoundException()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            PersonService service = new PersonService(uow.Object, emailService.Object);


            service.AddPersonsToTeam(It.IsAny<int[]>(), "1a");
        }

        [TestMethod]
        [ExpectedException(typeof(PersonNotFoundException))]
        public void AddPersonsToTeamMethod_NotAllTeamMembersContainsInDatabase_ShouldBeThrownPersonNotFoundException()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            PersonService service = new PersonService(uow.Object, emailService.Object);

            uow.Setup(x => x.People.Find(It.IsAny<Expression<Func<Person, bool>>>())).Returns(new List<Person>() { new Person() });


            service.AddPersonsToTeam(new int[] { 1, 2, 3 }, It.IsAny<string>());
        }

        [TestMethod]
        public void AddPersonsToTeamMethod_NewMembersAddingToDatabase_ShouldBeAddedNewTeamMembers()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            PersonService service = new PersonService(uow.Object, emailService.Object);

            uow.Setup(x => x.People.Find(It.IsAny<Expression<Func<Person, bool>>>())).Returns(new List<Person>() { new Person() { TeamId = 1 } });
            uow.Setup(x => x.People.Update(It.IsAny<int>(), It.IsAny<Person>()));
            uow.Setup(x => x.Teams.GetById(It.IsAny<int>())).Returns(new Team());


            service.AddPersonsToTeam(new int[] { 1 }, It.IsAny<string>());


            uow.Verify(x => x.Save());
        }

        #endregion

        #region GetTeam

        [TestMethod]
        [ExpectedException(typeof(ManagerNotFoundException))]
        public void GetTeamMethod_ManagerNotExists_ShouldBeThrownManagerNotFoundException()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            PersonService service = new PersonService(uow.Object, emailService.Object);

            uow.Setup(x => x.People.Find(It.IsAny<Expression<Func<Person, bool>>>())).Returns(new List<Person>());


            service.GetTeam(It.IsAny<string>());
        }

        #endregion

    }
}
