using BLL.DTO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.AccountModels;
using WebApi.Configurations.IntegrationTests;
using WebApi.IntegrationTests.Configurations;
using Xunit;

namespace WebApi.IntegrationTests.IntegrationTests
{
    public class AdminApiIntegrationTest : IClassFixture<TestFixture<Startup>>
    {
        private HttpClient client;

        public AdminApiIntegrationTest(TestFixture<Startup> fixture)
        {
            client = fixture.Client;
            client.UseToken("Nightmare", "Admin", "1a");
        }

        [Fact]
        public async Task Get_GetAllUsers_DoesReturnOk_UsersExist()
        {
            // Arrange
            var request = "/api/admin";

            // Act
            var response = await client.GetAsync(request);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<UserViewModel>>(jsonResult);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(4, actual.Count());
        }

        [Fact]
        public async Task Get_GetUser_DoesReturnOk_UserExist()
        {
            // Arrange
            var request = "/api/admin/2b";
            var expected = new UserDTO
            {
                Id = "2b",
                FName = "Jason",
                LName = "Voorhees",
                Email = "slasher@tms.com",
                UserName = "Slasher",
                Role = "Manager",
                TeamName = "Cups"
            };

            // Act
            var response = await client.GetAsync(request);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<UserDTO>(jsonResult);

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
        public async Task Post_UpdateRole_DoesReturnOk_RoleExists()
        {
            // Arrange
            var request = new
            {
                Url = "/api/admin/3c",
                Body = new RoleChangeViewModel
                {
                    Role = "Manager"
                }
            };

            // Act
            var response = await client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
