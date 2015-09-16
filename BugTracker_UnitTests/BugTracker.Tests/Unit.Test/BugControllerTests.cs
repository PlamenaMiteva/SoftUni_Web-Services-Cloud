using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using BugTracker.Data.Models;
using BugTracker.RestServices.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugTracker.RestServices.Models;

namespace BugTracker.Tests.Unit.Test
{
    [TestClass]
    public class BugControllerTests
    {
        [TestMethod]
        public void EditingExistingBug_ShouldChangeOnlySentData()
        {
            var fakeBugs = new List<Bug>
            {
                new Bug()
                {
                    Id = 1,
                    Title = "Bug 1",
                    Description = "bug 1 description"
                },
                new Bug()
                {
                    Id = 2,
                    Title = "Bug 2",
                    Description = "bug 2 description"
                }
            };
            var fakeRepo = new FakeBugsRepository(fakeBugs);
            var fakeUnitOfWork = new FakeUnitOfWork(fakeRepo);
            var newTitle = "Changed" + DateTime.Now.Ticks;
            var model = new EditBugBindingModel()
            {
                Title = newTitle
            };
            var oldDescription = fakeBugs[0].Description;
            var oldStatus = fakeBugs[0].Status;

            var controller = new BugsController(fakeUnitOfWork);
            SetupController(controller);
            var response = controller.EditBug(model, 1).ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(1, fakeUnitOfWork.SaveChangesCallCount);
            Assert.AreEqual(oldDescription, fakeBugs[0].Description);
            Assert.AreEqual(oldStatus, fakeBugs[0].Status);
            Assert.AreEqual(newTitle, fakeBugs[0].Title);
        }


        private void SetupController(ApiController controller)
        {
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
        }
    }
}
