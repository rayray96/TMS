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
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace BLL.Tests.ServicesTests
{
    [TestClass]
    public class StatusServiceTests
    {
        [TestMethod]
        public void DeleteTaskMethod_InvalidTaskId_ShouldBeThrownTaskNotFoundException()
        {
            IEnumerable<Status> statuses = new List<Status>()
            {
                new Status() { Name="Not started" },
                new Status() { Name="Completed" }
            };
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();
            IUnitOfWork uow = mock.Object;
            StatusService service = new StatusService(uow);

            mock.Setup(x => x.Statuses.GetAll()).Returns(statuses);


            var actual = service.GetAllStatuses();


            CollectionAssert.AreEqual(statuses.Select(x => x.Name).ToList(), actual.Select(x => x.Name).ToList());
        }

    }
}
