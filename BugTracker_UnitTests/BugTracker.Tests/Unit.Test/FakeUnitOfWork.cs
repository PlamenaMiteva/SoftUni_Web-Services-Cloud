using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Data.Models;
using BugTracker.Data.Repositories;
using BugTracker.Data.UnitOfWork;

namespace BugTracker.Tests.Unit.Test
{
    public class FakeUnitOfWork:IBugTrackerData
    {
        private IRepository<Bug> fakeBugRepository;

        public int saveChangesCallCount { get; set; }

        public int SaveChangesCallCount
        {
            get { return this.saveChangesCallCount; }
            private set { this.saveChangesCallCount = value; }
        }

        public FakeUnitOfWork(IRepository<Bug> fakeRepository)
        {
            this.fakeBugRepository = fakeRepository;
        }
        public Data.Repositories.IRepository<Data.Models.Bug> Bugs { get { return this.fakeBugRepository; } }

        public IRepository<Comment> Comments
        {
            get { throw new NotImplementedException(); }
        }

        public IRepository<User> Users
        {
            get { throw new NotImplementedException(); }
        }

        public int SaveChanges()
        {
            this.SaveChangesCallCount++;
            return this.SaveChangesCallCount;
        }
    }
}
