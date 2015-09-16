using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using BugTracker.Data.Models;

namespace BugTracker.RestServices.Models
{
    public class BugViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Status Status { get; set; }

        public AuthorViewModel BugAuthor { get; set; }

        public DateTime DateCreated { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public static Expression<Func<Bug, BugViewModel>> Create
        {
            get
            {
                return b => new BugViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    Status = b.Status,
                    BugAuthor = new AuthorViewModel()
                    {
                        Username = b.BugAuthor.UserName
                    },
                    DateCreated = b.SubmitDate,
                    Comments = b.Comments
                        .OrderBy(c => c.CreatedOn)
                        .Select(c => new CommentViewModel()
                        {
                            Id = c.Id,
                            Text = c.Text,
                            Author = c.CommentAuthor == null ? null : c.CommentAuthor.UserName,
                            DateCreated = c.CreatedOn
                        })
                };
            }
        }
    }
}