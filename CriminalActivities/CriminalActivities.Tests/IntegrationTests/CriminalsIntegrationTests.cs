using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using CriminalActivities.App;
using CriminalActivities.App.Models.View_Models;
using CriminalActivities.Data;
using CriminalActivities.Models;
using CriminalActivities.Models.Enums;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Owin;

namespace CriminalActivities.Tests.IntegrationTests
{
    [TestClass]
    public class CriminalsIntegrationTests
    {
        private const string TestUserUsername = "admin";
        private const string TestUserPassword = "Admin123";

        private static TestServer server;
        private static HttpClient client;

        private string accessToken;

        private string AccessToken
        {
            get
            {
                if (this.accessToken == null)
                {
                    var loginResponse = this.Login();
                    if (!loginResponse.IsSuccessStatusCode)
                    {
                        Assert.Fail("Unable to login: " + loginResponse.ReasonPhrase);
                    }

                    var loginData = loginResponse.Content
                        .ReadAsAsync<LoginDto>().Result;

                    this.accessToken = loginData.Access_Token;
                }

                return this.accessToken;
            }
        }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            // Create in-memory test server
            server = TestServer.Create(appBuilder =>
            {
                var config = new HttpConfiguration();
                WebApiConfig.Register(config);

                var startup = new Startup();
                startup.Configuration(appBuilder);

                appBuilder.UseWebApi(config);
            });

            client = server.HttpClient;

            // Use custom seed class
            var seedConfig = new SeedConfiguration(TestUserUsername, TestUserPassword);
            seedConfig.Seed();
        }

        [AssemblyCleanup]
        public static void Cleanup()
        {
            if (server != null)
            {
                server.Dispose();
            }
        }

        [TestMethod]
        public void RegisterCriminal_WithNewCorrectData_ShouldRegisterNewCriminal_And_Return201Created()
        {
            // Arrange
            var context = new CriminalContext();
            var newName = "Gaetano Mosca";
            var newAlias = "Gaetano";

            // Act
            this.SetAuthorizationHeaders(true);

            var response = this.SendCreateNewCriminalRequest(newName, newAlias);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            
            var criminal = response.Content.ReadAsAsync<List<RegisterdCriminalViewModel>>().Result;
            Assert.AreEqual(newName, criminal[0].Name);
            Assert.AreEqual(newAlias, criminal[0].Alias);
            Assert.AreEqual(Status.Active.ToString(), criminal[0].Status.ToString());
            Assert.AreEqual(TestUserUsername, criminal[0].RegisteredBy);
        }

        [TestMethod]
        public void RegisterCriminal_WithInCorrectData_ShouldReturnBadRequest()
        {
            // Arrange
            var context = new CriminalContext();
            var newName = "";
            var newAlias = "New Alias";

            // Act
            this.SetAuthorizationHeaders(true);

            var response = this.SendCreateNewCriminalRequest(newName, newAlias);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void RegisterCriminalWhoAlreadyExists_ShouldReturnConflict()
        {
            // Arrange
            var context = new CriminalContext();
            var existingCriminal = new Criminal()
            {
                FullName = "Mafioso",
                Alias = "Mafioso",

            };
            context.Criminals.Add(existingCriminal);
            context.SaveChanges();
            var newName = "Mafioso";
            var newAlias = "Mafioso";

            // Act
            this.SetAuthorizationHeaders(true);

            var response = this.SendCreateNewCriminalRequest(newName, newAlias);

            // Assert
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);

            var mafioso = context.Criminals.Where(c => c.FullName == "Mafioso");
            Assert.AreEqual(1, mafioso.Count());
        }

       

        [TestMethod]
        public void RegisterCriminal_WithoutAccessToken_ShouldReturn401Unauthorized()
        {
            // Arrange
            var context = new CriminalContext();
            var newName = "New name";
            var newAlias = "New Alias";

            // Act
            this.SetAuthorizationHeaders(false);

            var response = this.SendCreateNewCriminalRequest(newName, newAlias);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        private HttpResponseMessage SendCreateNewCriminalRequest(string newName, string newAlias)
        {
            var model = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("name", newName),
                new KeyValuePair<string, string>("alias", newAlias)
            });

            return client.PostAsync("api/criminals/", model).Result;
        }

        private HttpResponseMessage Login()
        {
            var loginData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", TestUserUsername),
                new KeyValuePair<string, string>("password", TestUserPassword),
                new KeyValuePair<string, string>("grant_type", "password")
            });

            var response = client.PostAsync("api/account/login", loginData).Result;

            return response;
        }

        private void SetAuthorizationHeaders(bool isLogged)
        {
            client.DefaultRequestHeaders.Remove("Authorization");
            if (isLogged)
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + this.AccessToken);
            }
        }
    }
}

