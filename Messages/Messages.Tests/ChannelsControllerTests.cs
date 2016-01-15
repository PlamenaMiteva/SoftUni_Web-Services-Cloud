using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Messages.Data;
using Messages.RestServices.Controllers;
using Messages.RestServices.Models.Binding_Models;
using Messages.RestServices.Models.View_Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Messages.Tests;

namespace Messages.Tests
{
    [TestClass]
    public class ChannelsControllerTests
    {
        private MockContainer mocks;
        [TestInitialize]
        public void InitTest()
        {
            this.mocks = new MockContainer();
            this.mocks.SetupMocks();
        }

        [TestMethod]
        public void GetChannelById_ShouldReturn200OK()
        {
            // Arrange
            var fakeData = this.mocks.MockData.Object;
            var existingChannel = this.mocks.ChannelsMock.Object.All()
                .First();
            var existingChannelId = existingChannel.Id;

            // Act
            var response = this.SendGetChannelsRequest(existingChannelId, fakeData);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var channels = response.Content
                .ReadAsAsync<IEnumerable<ChannelViewModel>>().Result;
            foreach (var channel in channels)
            {
                Assert.AreEqual(existingChannelId, channel.Id);
                Assert.AreEqual(existingChannel.Name, channel.Name);
            }
        }



        [TestMethod]
        public void GetNonExistingChannelById_ShouldReturn404NotFound()
        {
            // Arrange
            var fakeData = this.mocks.MockData.Object;
            var nonexistingChannelId = -1;

            // Act
            var response = this.SendGetChannelsRequest(nonexistingChannelId, fakeData);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            
        }
        private HttpResponseMessage SendGetChannelsRequest(int channelId, IMessagesData data)
        {
            var model = new SearchChannelsBindingModel { ChannelId = channelId };

            var controller = new ChannelsController(data);
            this.SetupController(controller);

            var response = controller.GetChannelById(model)
                .ExecuteAsync(CancellationToken.None).Result;
            return response;
        }

        private void SetupController(ApiController controller)
        {
            controller.Configuration = new HttpConfiguration();
            controller.Request = new HttpRequestMessage();
        }
    }
}
