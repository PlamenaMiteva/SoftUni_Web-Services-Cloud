using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using BugTracker.Data.Repositories;
using BugTracker.Data.UnitOfWork;
using BugTracker.RestServices.Controllers;
using BugTracker.RestServices.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BugTracker.Tests.Unit_Tests_Moq
{

    [TestClass]
    public class BugsControllerTestMoq
    {
        private MockContainer mocks;


        [TestInitialize]
        public void InitTests()
        {
            this.mocks = new MockContainer();
            this.mocks.PrepareMocks();
        }


        [TestMethod]
        public void EditingExistingBug_ShouldChangeOnlySentData_Moq()
        {
            var fakeBugs = this.mocks.BugRepositoryMock.Object.All();
            var mockContext = new Mock<IBugTrackerData>();
            mockContext.Setup(u => u.Bugs).Returns(mocks.BugRepositoryMock.Object);
            var bugsController = new BugsController(mockContext.Object);
            SetupController(bugsController);

            var newTitle = "Changed" + DateTime.Now.Ticks;
            var model = new EditBugBindingModel()
            {
                Title = newTitle
            };
            var unchangedBug = fakeBugs.First(b => b.Id == 1);
            var oldDescription = unchangedBug.Description;
            var oldStatus = unchangedBug.Status;

            var response = bugsController.EditBug(model, 1).ExecuteAsync(CancellationToken.None).Result;

            var changedBug = fakeBugs.First(b => b.Id == 1);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
            Assert.AreEqual(oldDescription, changedBug.Description);
            Assert.AreEqual(oldStatus, changedBug.Status);
            Assert.AreEqual(newTitle, changedBug.Title);
        }


        [TestMethod]
        public void EditingNonExistingBug_ShouldreturnNotFound_Moq()
        {
            var fakeBugs = this.mocks.BugRepositoryMock.Object.All();
            var mockContext = new Mock<IBugTrackerData>();
            mockContext.Setup(u => u.Bugs).Returns(mocks.BugRepositoryMock.Object);
            var bugsController = new BugsController(mockContext.Object);
            SetupController(bugsController);

            var newTitle = "Changed" + DateTime.Now.Ticks;
            var model = new EditBugBindingModel()
            {
                Title = newTitle
            };
            var response = bugsController.EditBug(model, 100).ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            mockContext.Verify(c => c.SaveChanges(), Times.Never);
        }


        private void SetupController(ApiController controller)
        {
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
        }
    }
}
