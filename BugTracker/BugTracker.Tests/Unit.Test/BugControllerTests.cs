using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BugTracker.RestServices.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugTracker.RestServices.Models;


namespace BugTracker.Tests.Unit.Test
{
    public class BugControllerTests
    {
        [TestMethod]
        public void EditingExistingBug_ShouldChangeOnlySentData()
        {
            var controller = new BugsController();
            var model = new EditBugBindingModel()
            {
                Title = "Changed"+DateTime.Now.Ticks
            };
            var response = controller.EditBug(model, 3).ExecuteAsync(CancellationToken.None).Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
