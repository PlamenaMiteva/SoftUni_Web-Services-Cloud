using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Messages.Data;
using Messages.RestServices;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Owin;

namespace Messages.Tests.IntegrationTests
{
    [TestClass]
    public class ChannelIntegrationTests
    {
        private const string TestUserUsername = "admin";
        private const string TestUserPassword = "Admin123";

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
            var seedConfig = new SeedConfiguration(TestUserUsername, TestUserPassword);
            seedConfig.Seed();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            if (server != null)
            {
                server.Dispose();
            }
        }

        [TestMethod]
        public void DeleteChannelWithoutMessages_ShouldReturn200OK()
        {
            var context = new MessagesDbContext();
            var channel = context.Channels.First(c=>c.Name=="Channel2");
            var response = this.SendDeleteChannelRequest(channel.Id);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }


        [TestMethod]
        public void DeleteChannelWithMessages_ShouldReturnConflict()
        {
            var context = new MessagesDbContext();
            var channel = context.Channels.FirstOrDefault(c => c.ChannelMessages.Any());
            var response = this.SendDeleteChannelRequest(channel.Id);
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
        }

        [TestMethod]
        public void DeleteNonExistingChannel_ShouldReturnNotFound()
        {
            var context = new MessagesDbContext();
            var channelId= -1;
            var response = this.SendDeleteChannelRequest(channelId);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        private HttpResponseMessage SendDeleteChannelRequest(int channelId)
        {
            return client.DeleteAsync("api/channels/" + channelId).Result;
        }

    }
}
