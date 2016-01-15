using System;
using System.Linq.Expressions;
using CriminalActivities.Models;

namespace CriminalActivities.App.Models.View_Models
{
    public class ActivityViewModel
    {
        public string Description { get; set; }

        public DateTime ActiveFrom { get; set; }

        public DateTime? ActiveTo { get; set; }

        public string Type { get; set; }

        public string Criminal { get; set; }

        public static Expression<Func<Activity, ActivityViewModel>> Create
        {
            get
            {
                return a => new ActivityViewModel()
                {
                    Description = a.Description,
                    ActiveFrom = a.ActiveFrom,
                    ActiveTo = a.ActiveTo,
                    Type = a.ActivityType.Name,
                    Criminal = a.Criminal.FullName
                };
            }
        }
    }
}