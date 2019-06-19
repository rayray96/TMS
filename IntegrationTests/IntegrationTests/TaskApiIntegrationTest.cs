using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.Configurations.IntegrationTests;
using WebApi.IntegrationTests.Configurations;
using WebApi.Models;
using Xunit;

namespace WebApi.IntegrationTests.IntegrationTests
{
    public class TaskApiIntegrationTest : IClassFixture<TestFixture<Startup>>
    {
        private HttpClient client;

        public TaskApiIntegrationTest(TestFixture<Startup> fixture)
        {
            client = fixture.Client;
        }

        [Fact]
        public async Task Get_GetStatuses_DoesReturnOk_StatusesExistForManager()
        {
            // Arrange
            client.UseToken("Slasher", "Manager", "2b");
            var request = "/api/task/statuses";

            // Act
            var response = await client.GetAsync(request);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<StatusViewModel>>(jsonResult);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(3, actual.Count());
        }

        [Fact]
        public async Task Get_GetStatuses_DoesReturnOk_StatusesExistForWorker()
        {
            // Arrange
            client.UseToken("Murder", "Worker", "3c");
            var request = "/api/task/statuses";

            // Act
            var response = await client.GetAsync(request);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<StatusViewModel>>(jsonResult);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(5, actual.Count());
        }

        [Fact]
        public async Task Get_GetTasksOfAuthor_DoesReturnOk_TasksExist()
        {
            // Arrange
            client.UseToken("Slasher", "Manager", "2b");
            var request = "/api/task/managerTasks/2b";


            // Act
            var response = await client.GetAsync(request);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<TaskViewModel>>(jsonResult);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(3, actual.Count());
        }

        [Fact]
        public async Task Get_GetTasksOfAssignee_DoesReturnOk_TasksExist()
        {
            // Arrange
            client.UseToken("Murder", "Worker", "3c");
            var request = "/api/task/workerTasks/3c";

            // Act
            var response = await client.GetAsync(request);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<TaskViewModel>>(jsonResult);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(3, actual.Count());
        }

        [Fact]
        public async Task Post_CreateTask_DoesReturnOk_GivenNewTask()
        {
            // Arrange
            client.UseToken("Slasher", "Manager", "2b");
            var request = new
            {
                Url = "/api/task",
                Body = new EditTaskViewModel
                {
                    AssigneeId = 3,
                    Description = "Bla bla bla... UI",
                    Name = "Develope WebApi",
                    Priority = "High",
                    Assignee = "Mike Myers",
                    Deadline = DateTime.Now.AddDays(10)
                }
            };

            // Act
            var response = await client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Put_UpdateStatus_DoesReturnOk_GivenNewStatusFromManager()
        {
            // Arrange
            client.UseToken("Slasher", "Manager", "2b");
            var request = new
            {
                Url = "/api/task/2b/status",
                Body = new EditStatusViewModel
                {
                    Status = "Completed",
                    TaskId = 4
                }
            };

            // Act
            var response = await client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Put_UpdateStatus_DoesReturnOk_GivenNewStatusFromWorker()
        {
            // Arrange
            client.UseToken("Murder", "Worker", "3c");
            var request = new
            {
                Url = "/api/task/3c/status",
                Body = new EditStatusViewModel
                {
                    Status = "In Progress",
                    TaskId = 2
                }
            };

            // Act
            var response = await client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Put_UpdateTask_DoesReturnOk_GivenUpdatedTask()
        {
            // Arrange
            client.UseToken("Slasher", "Manager", "2b");
            var request = new
            {
                Url = "/api/task/2",
                Body = new EditTaskViewModel
                {
                    AssigneeId = 3,
                    Description = "Bla bla bla... UI",
                    Name = "Develope WebApi",
                    Priority = "High",
                    Assignee = "Mike Myers",
                    Deadline = DateTime.Now.AddDays(10)
                }
            };

            // Act
            var response = await client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_DeleteTask_DoesReturnOk_TaskExists()
        {
            // Arrange
            client.UseToken("Slasher", "Manager", "2b");
            var request = "/api/task/3";

            // Act
            var response = await client.DeleteAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
