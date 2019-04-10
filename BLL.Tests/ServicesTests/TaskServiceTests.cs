using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
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
    public class TaskServiceTests
    {

        #region DeleteTask

        [TestMethod]
        [ExpectedException(typeof(TaskNotFoundException))]
        public void DeleteTaskMethod_InvalidTaskId_ShouldBeThrownTaskNotFoundException()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);

            uow.Setup(x => x.Tasks.GetById(It.IsAny<int>())).Returns((TaskInfo)null);


            service.DeleteTask(It.IsAny<int>(), It.IsAny<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(TaskAccessException))]
        public void DeleteTaskMethod_NoAuthorTask_ShouldBeThrownTaskAccessException()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);

            uow.Setup(x => x.Tasks.GetById(It.IsAny<int>())).Returns(new TaskInfo() { AuthorId = 1 });
            uow.Setup(x => x.People.Find(It.IsAny<Expression<Func<Person, bool>>>())).Returns(new List<Person>() { new Person() { Id = 2 } });


            service.DeleteTask(It.IsAny<int>(), It.IsAny<string>());
        }

        [TestMethod]
        public void DeleteTaskMethod_TaskExists_ShouldBeDeletedTask()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);

            uow.Setup(x => x.Tasks.GetById(It.IsAny<int>())).Returns(new TaskInfo() { AuthorId = 1 });
            uow.Setup(x => x.People.Find(It.IsAny<Expression<Func<Person, bool>>>())).Returns(new List<Person>() { new Person() { Id = 1 } });


            service.DeleteTask(It.IsAny<int>(), It.IsAny<string>());


            uow.Verify(x => x.Save());
        }

        #endregion

        #region CreateTask

        [TestMethod]
        [ExpectedException(typeof(DateIsWrongException))]
        public void CreateTaskMethod_DateLessThanToday_ShouldBeThrownDateIsWrongException()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);


            service.CreateTask(new EditTaskDTO() { Deadline = new DateTime(1900, 01, 01) }, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(PersonNotFoundException))]
        public void CreateTaskMethod_PersonNotExists_ShouldBeThrownPersonNotFoundException()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);

            uow.Setup(x => x.People.Find(It.IsAny<Expression<Func<Person, bool>>>())).Returns(new List<Person>());

            service.CreateTask(new EditTaskDTO() { Deadline = DateTime.Now.AddDays(1) }, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(PriorityNotFoundException))]
        public void CreateTaskMethod_PriorityNotExists_ShouldBeThrownPriorityNotFoundException()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);

            uow.Setup(x => x.People.Find(It.IsAny<Expression<Func<Person, bool>>>())).Returns(new List<Person>() { new Person() });
            uow.Setup(x => x.Priorities.Find(It.IsAny<Expression<Func<Priority, bool>>>())).Returns(new List<Priority>());


            service.CreateTask(new EditTaskDTO() { Deadline = DateTime.Now.AddDays(1) }, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(StatusNotFoundException))]
        public void CreateTaskMethod_StatusNotExists_ShouldBeThrownStatusNotFoundException()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);

            uow.Setup(x => x.People.Find(It.IsAny<Expression<Func<Person, bool>>>())).Returns(new List<Person>() { new Person() });
            uow.Setup(x => x.Priorities.Find(It.IsAny<Expression<Func<Priority, bool>>>())).Returns(new List<Priority>() { new Priority() });
            uow.Setup(x => x.Statuses.Find(It.IsAny<Expression<Func<Status, bool>>>())).Returns(new List<Status>());


            service.CreateTask(new EditTaskDTO() { Deadline = DateTime.Now.AddDays(1) }, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>());
        }

        [TestMethod]
        public void CreateTaskMethod_CreatingTask_ShouldBeCreatedTask()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);

            uow.Setup(x => x.People.Find(It.IsAny<Expression<Func<Person, bool>>>())).Returns(new List<Person>() { new Person() });
            uow.Setup(x => x.Priorities.Find(It.IsAny<Expression<Func<Priority, bool>>>())).Returns(new List<Priority>() { new Priority() });
            uow.Setup(x => x.Statuses.Find(It.IsAny<Expression<Func<Status, bool>>>())).Returns(new List<Status>() { new Status() });
            uow.Setup(x => x.Tasks.Create(It.IsAny<TaskInfo>()));


            service.CreateTask(new EditTaskDTO() { Deadline = DateTime.Now.AddDays(1) }, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>());


            uow.Verify(x => x.Save());
        }

        #endregion

        #region UpdateTask

        [TestMethod]
        [ExpectedException(typeof(TaskNotFoundException))]
        public void UpdateTaskMethod_InvalidTaskId_ShouldBeThrownTaskNotFoundException()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);

            uow.Setup(x => x.Tasks.GetById(It.IsAny<int>())).Returns((TaskInfo)null);


            service.UpdateTask(It.IsAny<EditTaskDTO>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(DateIsWrongException))]
        public void UpdateTaskMethod_DateLessThanToday_ShouldBeThrownDateIsWrongException()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);
            uow.Setup(x => x.Tasks.GetById(It.IsAny<int>())).Returns(new TaskInfo());


            service.UpdateTask(new EditTaskDTO() { Deadline = new DateTime(1900, 01, 01) }, It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(PersonNotFoundException))]
        public void UpdateTaskMethod_PersonNotExists_ShouldBeThrownPersonNotFoundException()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);

            uow.Setup(x => x.Tasks.GetById(It.IsAny<int>())).Returns(new TaskInfo());
            uow.Setup(x => x.People.Find(It.IsAny<Expression<Func<Person, bool>>>())).Returns(new List<Person>());

            service.UpdateTask(new EditTaskDTO() { Deadline = DateTime.Now.AddDays(1) }, It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(PriorityNotFoundException))]
        public void UpdateTaskMethod_PriorityNotExists_ShouldBeThrownPriorityNotFoundException()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);

            uow.Setup(x => x.Tasks.GetById(It.IsAny<int>())).Returns(new TaskInfo());
            uow.Setup(x => x.People.Find(It.IsAny<Expression<Func<Person, bool>>>())).Returns(new List<Person>() { new Person() });
            uow.Setup(x => x.Priorities.Find(It.IsAny<Expression<Func<Priority, bool>>>())).Returns(new List<Priority>());


            service.UpdateTask(new EditTaskDTO() { Deadline = DateTime.Now.AddDays(1) }, It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(StatusNotFoundException))]
        public void UpdateTaskMethod_StatusNotExists_ShouldBeThrownStatusNotFoundException()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);

            uow.Setup(x => x.Tasks.GetById(It.IsAny<int>())).Returns(new TaskInfo());
            uow.Setup(x => x.People.Find(It.IsAny<Expression<Func<Person, bool>>>())).Returns(new List<Person>() { new Person() });
            uow.Setup(x => x.Priorities.Find(It.IsAny<Expression<Func<Priority, bool>>>())).Returns(new List<Priority>() { new Priority() });
            uow.Setup(x => x.Statuses.Find(It.IsAny<Expression<Func<Status, bool>>>())).Returns(new List<Status>());


            service.UpdateTask(new EditTaskDTO() { Deadline = DateTime.Now.AddDays(1) }, It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>());
        }

        [TestMethod]
        public void UpdateTaskMethod_CreatingTask_ShouldBeUpdatedTask()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);

            uow.Setup(x => x.Tasks.GetById(It.IsAny<int>())).Returns(new TaskInfo());
            uow.Setup(x => x.People.Find(It.IsAny<Expression<Func<Person, bool>>>())).Returns(new List<Person>() { new Person() });
            uow.Setup(x => x.Priorities.Find(It.IsAny<Expression<Func<Priority, bool>>>())).Returns(new List<Priority>() { new Priority() });
            uow.Setup(x => x.Statuses.Find(It.IsAny<Expression<Func<Status, bool>>>())).Returns(new List<Status>() { new Status() });
            uow.Setup(x => x.Tasks.Update(It.IsAny<int>(), It.IsAny<TaskInfo>()));


            service.UpdateTask(new EditTaskDTO() { Deadline = DateTime.Now.AddDays(1) }, It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>());


            uow.Verify(x => x.Save());
        }

        #endregion

        #region UpdateStatus

        [TestMethod]
        [ExpectedException(typeof(StatusNotFoundException))]
        public void UpdateStatusMethod_InvalidStatusName_ShouldBeThrownStatusNotFoundException()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);

            uow.Setup(x => x.Statuses.Find(It.IsAny<Expression<Func<Status, bool>>>())).Returns(new List<Status>());


            service.UpdateStatus(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>());
        }

        [TestMethod]
        [ExpectedException(typeof(TaskNotFoundException))]
        public void UpdateStatusMethod_InvalidTaskId_ShouldBeThrownTaskNotFoundException()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);

            uow.Setup(x => x.Statuses.Find(It.IsAny<Expression<Func<Status, bool>>>())).Returns(new List<Status>() { new Status() });
            uow.Setup(x => x.Tasks.GetById(It.IsAny<int>())).Returns((TaskInfo)null);


            service.UpdateStatus(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>());
        }

        [TestMethod]
        [ExpectedException(typeof(StatuskAccessException))]
        public void UpdateStatusMethod_AccessToClosedStatusesOfManager_ShouldBeThrownStatuskAccessException()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);

            uow.Setup(x => x.Statuses.Find(It.IsAny<Expression<Func<Status, bool>>>())).Returns(new List<Status>() { new Status() });
            uow.Setup(x => x.Tasks.GetById(It.IsAny<int>())).Returns(new TaskInfo() { AuthorId = 1 });
            uow.Setup(x => x.Statuses.GetById(It.IsAny<int>())).Returns(new Status());


            service.UpdateStatus(It.IsAny<int>(), It.IsAny<string>(), 1);
        }

        [TestMethod]
        public void UpdateStatusMethod_ManagerChangingStatus_ShouldBeUpdateStatus()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);

            uow.Setup(x => x.Statuses.Find(It.IsAny<Expression<Func<Status, bool>>>())).Returns(new List<Status>() { new Status() });
            uow.Setup(x => x.Tasks.GetById(It.IsAny<int>())).Returns(new TaskInfo() { AuthorId = 1 });
            uow.Setup(x => x.Statuses.GetById(It.IsAny<int>())).Returns(new Status());
            uow.Setup(x => x.Tasks.Update(It.IsAny<int>(), It.IsAny<TaskInfo>()));


            service.UpdateStatus(It.IsAny<int>(), "Canceled", 1);


            uow.Verify(x => x.Save());
        }

        [TestMethod]
        [ExpectedException(typeof(StatuskAccessException))]
        public void UpdateStatusMethod_AccessToClosedStatusesOfWorker_ShouldBeThrownStatuskAccessException()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);

            uow.Setup(x => x.Statuses.Find(It.IsAny<Expression<Func<Status, bool>>>())).Returns(new List<Status>() { new Status() });
            uow.Setup(x => x.Tasks.GetById(It.IsAny<int>())).Returns(new TaskInfo() { AssigneeId = 1 });
            uow.Setup(x => x.Statuses.GetById(It.IsAny<int>())).Returns(new Status());


            service.UpdateStatus(It.IsAny<int>(), It.IsAny<string>(), 1);
        }

        [TestMethod]
        public void UpdateStatusMethod_WorkerChangingStatus_ShouldBeUpdateStatus()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);

            uow.Setup(x => x.Statuses.Find(It.IsAny<Expression<Func<Status, bool>>>())).Returns(new List<Status>() { new Status() });
            uow.Setup(x => x.Tasks.GetById(It.IsAny<int>())).Returns(new TaskInfo() { AssigneeId = 1 });
            uow.Setup(x => x.Statuses.GetById(It.IsAny<int>())).Returns(new Status());
            uow.Setup(x => x.Tasks.Update(It.IsAny<int>(), It.IsAny<TaskInfo>()));


            service.UpdateStatus(It.IsAny<int>(), "Not started", 1);


            uow.Verify(x => x.Save());
        }

        [TestMethod]
        [ExpectedException(typeof(StatuskAccessException))]
        public void UpdateStatusMethod_AccessToClosedStatusesForPerson_ShouldBeThrownStatuskAccessException()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);

            uow.Setup(x => x.Statuses.Find(It.IsAny<Expression<Func<Status, bool>>>())).Returns(new List<Status>() { new Status() });
            uow.Setup(x => x.Tasks.GetById(It.IsAny<int>())).Returns(new TaskInfo());
            uow.Setup(x => x.Statuses.GetById(It.IsAny<int>())).Returns(new Status());


            service.UpdateStatus(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>());
        }

        #endregion

        #region GetTask

        [TestMethod]
        public void GetTaskMethod_TaskExists_ShouldBeReturnedTask()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);

            var taskInfo = new TaskInfo()
            {
                PriorityId = 1,
                StatusId = 1,
                AssigneeId = 1,
                AuthorId = 2
            };

            uow.Setup(x => x.Tasks.Find(It.IsAny<Expression<Func<TaskInfo, bool>>>())).Returns(new List<TaskInfo>() { taskInfo });
            uow.Setup(x => x.People.GetAll()).Returns(new List<Person>() { new Person() { Id = 1 }, new Person() { Id = 2 } });
            uow.Setup(x => x.Statuses.GetAll()).Returns(new List<Status>() { new Status() { Id = 1 } });
            uow.Setup(x => x.Priorities.GetAll()).Returns(new List<Priority>() { new Priority() { Id = 1 } });


            var actual = service.GetTask(It.IsAny<int>());


            Assert.IsNotNull(actual);
        }


        #endregion

        #region GetAllTasks

        [TestMethod]
        public void GetAllTasksMethod_TasksExist_ShouldBeReturnedAllTasks()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);

            var taskInfo = new TaskInfo()
            {
                PriorityId = 1,
                StatusId = 1,
                AssigneeId = 1,
                AuthorId = 2
            };

            uow.Setup(x => x.Tasks.GetAll()).Returns(new List<TaskInfo>() { taskInfo });
            uow.Setup(x => x.People.GetAll()).Returns(new List<Person>() { new Person() { Id = 1 }, new Person() { Id = 2 } });
            uow.Setup(x => x.Statuses.GetAll()).Returns(new List<Status>() { new Status() { Id = 1 } });
            uow.Setup(x => x.Priorities.GetAll()).Returns(new List<Priority>() { new Priority() { Id = 1 } });


            var actual = service.GetAllTasks();


            Assert.AreEqual(1, actual.Count());
        }

        #endregion

        #region GetTasksOfAuthor

        [TestMethod]
        [ExpectedException(typeof(ManagerNotFoundException))]
        public void GetTasksOfAuthorMethod_InvalidManagerId_ShouldBeManagerNotFoundException()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);

            uow.Setup(x => x.People.Find(It.IsAny<Expression<Func<Person, bool>>>())).Returns(new List<Person>());


            service.GetTasksOfAuthor(It.IsAny<string>());
        }

        #endregion

        #region GetTasksOfAssignee

        [TestMethod]
        [ExpectedException(typeof(WorkerNotFoundException))]
        public void GetTasksOfAssigneeMethod_InvalidManagerId_ShouldBeWorkerNotFoundException()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IEmailService> emailService = new Mock<IEmailService>();
            TaskService service = new TaskService(uow.Object, emailService.Object);

            uow.Setup(x => x.People.Find(It.IsAny<Expression<Func<Person, bool>>>())).Returns(new List<Person>());


            service.GetTasksOfAssignee(It.IsAny<string>());
        }

        #endregion

    }
}
