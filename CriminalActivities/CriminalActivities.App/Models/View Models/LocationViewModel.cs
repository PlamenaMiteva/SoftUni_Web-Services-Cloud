using System;
using System.Linq.Expressions;
using CriminalActivities.Models;

namespace CriminalActivities.App.Models.View_Models
{
    public class LocationViewModel
    {
        public DateTime LastSeen { get; set; }

        public string City { get; set; }

        public string Criminal { get; set; }

        public static Expression<Func<Location, LocationViewModel>> Create
        {
            get
            {
                return l => new LocationViewModel()
                {
                    LastSeen = l.LastSeen,
                    City = l.City.Name,
                    Criminal = l.Criminal.FullName
                };
            }
        }
    }
}