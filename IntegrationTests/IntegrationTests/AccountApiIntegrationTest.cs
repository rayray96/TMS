using System.Net.Http;
using System.Threading.Tasks;
using WebApi.AccountModels;
using WebApi.Configurations.IntegrationTests;
using WebApi.IntegrationTests.Configurations;
using Xunit;

namespace WebApi.IntegrationTests.IntegrationTests
{
    public class AccountApiIntegrationTest : IClassFixture<TestFixture<Startup>>
    {
        private HttpClient client;

        public AccountApiIntegrationTest(TestFixture<Startup> fixture)
        {
            client = fixture.Client;
        }

        [Fact]
        public async Task Post_SignUpAsync_DoesReturnOk_GivenNewUser()
        {
            // Arrange
            var request = new
            {
                Url = "/api/account/sign-up",
                Body = new RegisterViewModel
                {
                    FName = "John",
                    LName = "Wick",
                    Email = "wick@tms.com",
                    UserName = "Mister_Wick",
                    Password = "qwerty123!",
                    ConfirmPassword = "qwerty123!"
                }
            };

            // Act
            var response = await client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Post_SignInAsync_DoesReturnOk_GivenUserCredentials()
        {
            // Arrange
            var request = new
            {
                Url = "/api/account/sign-in",
                Body = new LoginViewModel
                {
                    UserName = "Nightmare",
                    Password = "qwerty123!"
                }
            };

            // Act
            var response = await client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Post_GetRefreshTokenAsync_DoesReturnOk_GivenRefreshToken()
        {
            // Arrange
            client.UseToken("Nightmare", "Admin", "1a");
            var refreshToken = "1a2b3c4d5e";
            var request = $"/api/account/{refreshToken}/refresh";

            // Act
            var response = await client.PostAsync(request, ContentHelper.GetStringContent(refreshToken));

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
