using BugTracker.Data.Models;
using BugTracker.Data.Repositories;

namespace BugTracker.Data.UnitOfWork
{
    public interface IBugTrackerData
    {
        IRepository<Bug> Bugs { get; }

        IRepository<Comment> Comments { get; }

        IRepository<User> Users { get; }

        int SaveChanges();
    }
}
