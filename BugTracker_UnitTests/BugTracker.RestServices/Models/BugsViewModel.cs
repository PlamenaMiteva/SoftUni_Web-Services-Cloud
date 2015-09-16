using System;
using System.Linq.Expressions;
using BugTracker.Data.Models;

namespace BugTracker.RestServices.Models
{
    public class BugsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Status { get; set; }

        public string Author { get; set; }

        public DateTime DateCreated { get; set; }

        public static Expression<Func<Bug, BugsViewModel>> Create
        {
            get
            {
                return b => new BugsViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Status = b.Status.ToString(),
                    Author =  b.BugAuthor == null ? null : b.BugAuthor.UserName,
                    DateCreated = b.SubmitDate
                };
            }
        }
    }
}