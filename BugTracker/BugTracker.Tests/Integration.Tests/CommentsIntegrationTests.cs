using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BugTracker.Data;
using BugTracker.Data.Models;
using BugTracker.RestServices;
using BugTracker.RestServices.Models;
using Microsoft.Owin.Builder;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Owin;

namespace BugTracker.Tests.Integration.Tests
{
    [TestClass]
    public class CommentsIntegrationTests
    {
        private static TestServer server;
        private static HttpClient client;

        [AssemblyInitialize]
        public static void InitializeTests(TestContext context)
        {
            server = TestServer.Create(AppBuilder =>
            {
                var httpConfig = new HttpConfiguration();
                WebApiConfig.Register(httpConfig);
                var webApiStartup = new Startup();
                webApiStartup.Configuration(AppBuilder);
                AppBuilder.UseWebApi(httpConfig);
            });
            client = server.HttpClient;
            Seed();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            if (server != null)
            {
                server.Dispose();
            }
        }

        private static void Seed()
        {
            var context = new BugTrackerDbContext();
            var comment =new Comment()
            {
                Text = "First comment",
                CreatedOn = DateTime.Now
            };
            context.Comments.Add(comment);
            var bug = new Bug()
            {
                Title = "First bug",
                Description = "Bug 1",
                SubmitDate = DateTime.Now
            };
            bug.Comments.Add(comment);
            context.Bugs.Add(bug);
            context.SaveChanges();
        }

        [TestMethod]
        public void GetBugComments_ShouldReturn200OK_ExistingBug()
        {
            var dbContext = new BugTrackerDbContext();
            var existingBug = dbContext.Bugs.FirstOrDefault(b=>b.Comments.Any());
            if (existingBug == null)
            {
                Assert.Fail("Cannot perform test - no bugs in DB.");
            }
            var endpoint = string.Format("api/bugs/{0}/comments", existingBug.Id);
            var response = client.GetAsync(endpoint).Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var comments = response.Content.ReadAsAsync<List<CommentViewModel>>().Result;
            foreach (var comment in comments)
            {
                Assert.IsNotNull(comment.Text);
                Assert.AreNotEqual(0, comment.Id);
            }
        }

        [TestMethod]
        public void GetBugComments_ShouldReturn404NotFound_NonExistingBug()
        {
            var endpoint = string.Format("api/bugs/{0}/comments", -1);
            var response = client.GetAsync(endpoint).Result;
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
          }
    }
}
