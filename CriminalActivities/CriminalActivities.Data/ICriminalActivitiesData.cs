using CriminalActivities.Data.Repositories;
using CriminalActivities.Models;

namespace CriminalActivities.Data
{
    public  interface ICriminalActivitiesData
    {
        IRepository<Cartel> Cartels { get; }

        IRepository<User> Users { get; }

        IRepository<Location> Locations { get; }

        IRepository<Criminal> Criminals { get; }

        IRepository<City> Cities { get; }

        IRepository<Activity> Activities { get; }

        IRepository<ActivityType> ActivityTypes { get; }

        int SaveChanges();
    }
}
