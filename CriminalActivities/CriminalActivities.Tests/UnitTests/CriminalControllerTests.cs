using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using CriminalActivities.App.Controllers;
using CriminalActivities.App.Models.Binding_Models;
using CriminalActivities.App.Models.View_Models;
using CriminalActivities.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CriminalActivities.Tests.UnitTests
{
    [TestClass]
    public class CriminalControllerTests
    {
        private MockContainer mocks;

        [TestInitialize]
        public void InitTest()
        {
            this.mocks = new MockContainer();
            this.mocks.SetupMocks();
        }

        [TestMethod]
        public void GetCriminalByName_ShouldReturnCriminal_And200OK()
        {
            // Arrange
            var fakeData = this.mocks.MockData.Object;
            var existingCriminal = this.mocks.CriminalsMock.Object.All().First();
            var existingCriminalName = existingCriminal.FullName.ToString();

            // Act
            var response = this.SendGetCriminalByNameRequest(existingCriminalName, fakeData);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var criminal = response.Content
                .ReadAsAsync<List<CriminalViewModel>>().Result;
            Assert.AreEqual(existingCriminalName, criminal[0].Name);
            Assert.AreEqual(existingCriminal.Alias, criminal[0].Alias);
            Assert.AreEqual(existingCriminal.BirthDate, criminal[0].BirthDate);
            Assert.AreEqual(existingCriminal.Status, criminal[0].Status);
        }

        [TestMethod]
        public void GetCriminalByNameAndAlias_ShouldReturnCriminal_And200OK()
        {
            // Arrange
            var fakeData = this.mocks.MockData.Object;
            var existingCriminal = this.mocks.CriminalsMock.Object.All().First();
            var existingCriminalName = existingCriminal.FullName.ToString();
            var existingCriminalAlias = existingCriminal.Alias.ToString();
            // Act
            var response = this.SendGetCriminalByNameAndAliasRequest(existingCriminalName, existingCriminalAlias, fakeData);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var criminal = response.Content
                .ReadAsAsync<List<CriminalViewModel>>().Result;
            Assert.AreEqual(existingCriminalName, criminal[0].Name);
            Assert.AreEqual(existingCriminal.Alias, criminal[0].Alias);
            Assert.AreEqual(existingCriminal.BirthDate, criminal[0].BirthDate);
            Assert.AreEqual(existingCriminal.Status, criminal[0].Status);
        }

        [TestMethod]
        public void GetCriminalByMissingName_ShouldReturnCriminal_And200OK()
        {
            // Arrange
            var fakeData = this.mocks.MockData.Object;
            var existingCriminal = this.mocks.CriminalsMock.Object.All().First();
            var existingCriminalAlias = existingCriminal.Alias.ToString();
            // Act
            var response = this.SendGetCriminalByAliasRequest(existingCriminalAlias, fakeData);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void GetCriminalByNonExistingName_ShouldReturnNotFound()
        {
            // Arrange
            var fakeData = this.mocks.MockData.Object;
            var name = "Non existing Name";
            // Act
            var response = this.SendGetCriminalByNameRequest(name, fakeData);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
           
        }

        private HttpResponseMessage SendGetCriminalByNameRequest(string name, ICriminalActivitiesData data)
        {
            var model = new CriminalBindingModel { Name = name};

            var controller = new CriminalsController(data);
            this.SetupController(controller);

            var response = controller.GetCriminal(model)
                .ExecuteAsync(CancellationToken.None).Result;
            return response;
        }

        private HttpResponseMessage SendGetCriminalByNameAndAliasRequest(string name, string alias, ICriminalActivitiesData data)
        {
            var model = new CriminalBindingModel { Name = name, Alias = alias};

            var controller = new CriminalsController(data);
            this.SetupController(controller);

            var response = controller.GetCriminal(model)
                .ExecuteAsync(CancellationToken.None).Result;
            return response;
        }

        private HttpResponseMessage SendGetCriminalByAliasRequest(string alias, ICriminalActivitiesData data)
        {
            var model = new CriminalBindingModel { Alias = alias };

            var controller = new CriminalsController(data);
            this.SetupController(controller);

            var response = controller.GetCriminal(model)
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
