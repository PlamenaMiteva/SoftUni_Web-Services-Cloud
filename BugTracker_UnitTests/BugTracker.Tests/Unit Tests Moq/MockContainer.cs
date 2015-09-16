using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Data.Models;
using BugTracker.Data.Repositories;
using Moq;

namespace BugTracker.Tests.Unit_Tests_Moq
{
    public class MockContainer
    {
        public Mock<IRepository<Comment>> CommentRepositoryMock { get; set; }

        public Mock<IRepository<Bug>> BugRepositoryMock { get; set; }

        public void PrepareMocks()
        {
            this.SetupFakeBugs();

        }

        private void SetupFakeBugs()
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
            this.BugRepositoryMock = new Mock<IRepository<Bug>>();

            this.BugRepositoryMock.Setup(r => r.All()).Returns(fakeBugs.AsQueryable);
            this.BugRepositoryMock.Setup(r => r.Find(It.IsAny<int>())).Returns((int id) =>fakeBugs.FirstOrDefault(b=>b.Id==id));

        }

    }
}
