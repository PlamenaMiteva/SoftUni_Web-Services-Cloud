using System;
using System.Collections.Generic;
using System.Data.Entity;
using CriminalActivities.Data.Repositories;
using CriminalActivities.Models;

namespace CriminalActivities.Data
{
    public class CriminalData : ICriminalActivitiesData
    {
        private DbContext context;
        private IDictionary<Type, object> repositories;

        public CriminalData(DbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<User> Users
        {
            get { return this.GetRepository<User>(); }
        }

        public IRepository<Cartel> Cartels
        {
            get { return this.GetRepository<Cartel>(); }
        }

        public IRepository<City> Cities
        {
            get { return this.GetRepository<City>(); }
        }

        public IRepository<Location> Locations
        {
            get { return this.GetRepository<Location>(); }
        }

        public IRepository<Activity> Activities
        {
            get { return this.GetRepository<Activity>(); }
        }

        public IRepository<ActivityType> ActivityTypes
        {
            get { return this.GetRepository<ActivityType>(); }
        }

        public IRepository<Criminal> Criminals
        {
            get { return this.GetRepository<Criminal>(); }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        public int SaveChangesAsync()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var type = typeof(T);
            if (!this.repositories.ContainsKey(type))
            {
                var typeOfRepository = typeof(GenericRepository<T>);
                var repository = Activator.CreateInstance(typeOfRepository, this.context);

                this.repositories.Add(type, repository);
            }

            return (IRepository<T>)this.repositories[type];
        }
    }
}

