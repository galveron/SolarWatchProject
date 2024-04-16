using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using SolarWatchProject.Contracts;
using SolarWatchProject.Models;
using SolarWatchProject.Services;
using SolarWatchProject.Services.Authentication;

namespace SolarWatchProjectIntegrationTests
{
    [Collection("Integration")]
    public class AuthControllerTest
    {
        [Fact]
        public async Task UserRegistrationSuccessfullyTest()
        {
            var application = new SolarWatchWebApplicationFactory();
            var request = new RegistrationRequest("valaki@g.com", "valaki", "Valaki123456");

            var client = application.CreateClient();

            var response = await client.PostAsJsonAsync("/Auth/Register", request);

            response.EnsureSuccessStatusCode();

            var authResponse = await response.Content.ReadFromJsonAsync<RegistrationResponse>();

            Assert.Equal("valaki", authResponse.UserName);
        }

        [Fact]
        public async Task UserRegistrationWrongPasswordTest()
        {
            var application = new SolarWatchWebApplicationFactory();
            var request = new RegistrationRequest("valaki@g.com", "valaki", "Va");

            var client = application.CreateClient();

            var response = await client.PostAsJsonAsync("/Auth/Register", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UserLoginSuccessfullyTest()
        {
            var application = new SolarWatchWebApplicationFactory();
            const string testEmail = "valaki@gm.com";
            const string testPassword = "Valaki123456";
            const string testName = "valakik";
            var registrationRequest = new RegistrationRequest(testEmail, testName, testPassword);
            var loginRequest = new AuthRequest(testEmail, testPassword);

            var client = application.CreateClient();

            var registerResponse = await client.PostAsJsonAsync("/Auth/Register", registrationRequest);
            registerResponse.EnsureSuccessStatusCode();

            var loginResponse = await client.PostAsJsonAsync("/Auth/Login", loginRequest);
            loginResponse.EnsureSuccessStatusCode();

            var getUserRes = await client.GetAsync($"/User/GetUser?userName={testName}");
            getUserRes.EnsureSuccessStatusCode();
            var user = getUserRes.Content.ReadFromJsonAsync<User>();
            Assert.Equal(testEmail, user.Result.Email);
        }

        [Fact]
        public async Task UserLoginNotSuccessfullyTest()
        {
            var application = new SolarWatchWebApplicationFactory();
            const string testEmail = "valaki@gm.com";
            const string testPassword = "Valaki123456";
            const string testName = "valakik";
            var registrationRequest = new RegistrationRequest(testEmail, testName, testPassword);
            var loginRequest = new AuthRequest("wrong@email.hu", testPassword);

            var client = application.CreateClient();

            var registerResponse = await client.PostAsJsonAsync("/Auth/Register", registrationRequest);
            registerResponse.EnsureSuccessStatusCode();

            var loginResponse = await client.PostAsJsonAsync("/Auth/Login", loginRequest);

            var statusCode = loginResponse.StatusCode.ToString();
            Assert.Equal("BadRequest", statusCode);
        }

        [Fact]
        public async Task UserLogoutSuccessfullyTest()
        {
            var application = new SolarWatchWebApplicationFactory();
            const string testEmail = "valaki@gm.com";
            const string testPassword = "Valaki123456";
            const string testName = "valakik";
            var registrationRequest = new RegistrationRequest(testEmail, testName, testPassword);
            var loginRequest = new AuthRequest(testEmail, testPassword);

            var client = application.CreateClient();

            var registerResponse = await client.PostAsJsonAsync("/Auth/Register", registrationRequest);
            registerResponse.EnsureSuccessStatusCode();

            await client.PostAsJsonAsync("/Auth/Login", loginRequest);
            var logoutResult = await client.PostAsJsonAsync("/Auth/Logout", loginRequest);

            
            var statusCode = logoutResult.StatusCode.ToString();
            Assert.Equal("OK", statusCode);
        }
    }
}
