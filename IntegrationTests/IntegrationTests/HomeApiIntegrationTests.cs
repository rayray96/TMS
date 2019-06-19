using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.AccountModels;
using WebApi.Configurations.IntegrationTests;
using WebApi.IntegrationTests.Configurations;
using WebApi.Models;
using Xunit;

namespace WebApi.IntegrationTests.IntegrationTests
{
    public class HomeApiIntegrationTests : IClassFixture<TestFixture<Startup>>
    {
        private HttpClient client;

        public HomeApiIntegrationTests(TestFixture<Startup> fixture)
        {
            client = fixture.Client;
            client.UseToken("Nightmare", "Admin", "1a");
        }

        [Fact]
        public async Task Get_GetUserProfile_DoesReturnOk_GivenInformationFromContext()
        {
            // Arrange
            var url = "/api/home/userProfile";
            var expected = new UserViewModel
            {
                Id = "1a",
                Role = "Admin",
                FName = "Stephen",
                LName = "King",
                Email = "nightmare@tms.com",
                UserName = "Nightmare"
            };

            // Act
            var response = await client.GetAsync(url);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<UserViewModel>(jsonResult);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.FName, actual.FName);
            Assert.Equal(expected.LName, actual.LName);
            Assert.Equal(expected.Role, actual.Role);
            Assert.Equal(expected.UserName, actual.UserName);
            Assert.Equal(expected.Role, actual.Role);
            Assert.Equal(expected.TeamName, actual.TeamName);
        }

        [Fact]
        public async Task Get_AllTasks_DoesReturnOk_GivenInformationFromContext()
        {
            // Arrange
            var url = "/api/home/tasks";

            // Act
            var response = await client.GetAsync(url);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<TaskViewModel>>(jsonResult);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(3, actual.Count());
        }
    }
}
