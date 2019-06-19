using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.Configurations.IntegrationTests;
using WebApi.IntegrationTests.Configurations;
using WebApi.Models;
using Xunit;

namespace WebApi.IntegrationTests.IntegrationTests
{
    public class TeamApiIntegrationTests : IClassFixture<TestFixture<Startup>>
    {
        private HttpClient client;

        public TeamApiIntegrationTests(TestFixture<Startup> fixture)
        {
            client = fixture.Client;
        }

        [Fact]
        public async Task Get_GetMyTeam_DoesReturnOk_TeamExists()
        {
            // Arrange
            client.UseToken("Slasher", "Manager", "2b");
            var request = "/api/team/2b";
            var expected = new TeamViewModel
            {
                TeamName = "Cups",
                Team = new PersonViewModel[]
                    {
                        new PersonViewModel
                        {
                        Id = 4,
                        FName = "Freddy",
                        LName = "Krueger",
                        Email = "krueger@tms.com",
                        UserName = "Killer",
                        Role = "Worker",
                        TeamId = 7
                        }
                    }
            };
            var expectedPerson = expected.Team.ToArray();

            // Act
            var response = await client.GetAsync(request);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<TeamViewModel>(jsonResult);
            var actualPerson = actual.Team.ToArray();

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(expected.TeamName, actual.TeamName);
            Assert.Equal(1, actual.Team.Count);

            Assert.Equal(expectedPerson[0].Id, actualPerson[0].Id);
            Assert.Equal(expectedPerson[0].FName, actualPerson[0].FName);
            Assert.Equal(expectedPerson[0].LName, actualPerson[0].LName);
            Assert.Equal(expectedPerson[0].Role, actualPerson[0].Role);
            Assert.Equal(expectedPerson[0].UserName, actualPerson[0].UserName);
            Assert.Equal(expectedPerson[0].TeamId, actualPerson[0].TeamId);
            Assert.Equal(expectedPerson[0].Email, actualPerson[0].Email);
        }

        [Fact]
        public async Task Get_GetPossibleMembers_DoesReturnOk_PossibleMembersExist()
        {
            // Arrange
            client.UseToken("Slasher", "Manager", "2b");
            var request = "/api/team/possibleMembers";
            var expected = new PersonViewModel[]
            {
                new PersonViewModel
                {
                    Id = 3,
                    FName = "Mike",
                    LName = "Myers",
                    Email = "murder@tms.com",
                    UserName = "Murder",
                    Role = "Worker",
                }
            };

            // Act
            var response = await client.GetAsync(request);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<PersonViewModel[]>(jsonResult);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Single(actual);

            Assert.Equal(expected[0].Id, actual[0].Id);
            Assert.Equal(expected[0].FName, actual[0].FName);
            Assert.Equal(expected[0].LName, actual[0].LName);
            Assert.Equal(expected[0].Role, actual[0].Role);
            Assert.Equal(expected[0].UserName, actual[0].UserName);
            Assert.Equal(expected[0].TeamId, actual[0].TeamId);
            Assert.Equal(expected[0].Email, actual[0].Email);
        }

        [Fact]
        public async Task Post_CreateTeam_DoesReturnOk_GivenNemTeamName()
        {
            // Arrange
            client.UseToken("Assassin", "Manager", "5e");
            var request = new
            {
                Url = "/api/team/5e",
                Body = new TeamNameViewModel
                {
                    TeamName = "Plates"
                }
            };

            // Act
            var response = await client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Post_AddMembersToTeam_DoesReturnOk_GivenBookedWorkers()
        {
            // Arrange
            client.UseToken("Slasher", "Manager", "2b");
            var request = new
            {
                Url = "/api/team/addMembers/2b",
                Body = new AddMembersViewModel
                {
                    Members = new int[] { 4 }
                }
            };

            // Act
            var response = await client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Put_UpdateTeamName_DoesReturnOk_GivenNemTeamName()
        {
            // Arrange
            client.UseToken("Slasher", "Manager", "2b");
            var request = new
            {
                Url = "/api/team/2b",
                Body = new TeamNameViewModel
                {
                    TeamName = "Plates"
                }
            };

            // Act
            var response = await client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_DeleteFromTeam_DoesReturnOk_TeamExists()
        {
            // Arrange
            client.UseToken("Slasher", "Manager", "2b");
            var request  = "/api/team/3";

            // Act
            var response = await client.DeleteAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

    }
}
