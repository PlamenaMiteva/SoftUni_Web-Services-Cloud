using BugTracker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BugTracker.RestServices.Models
{
    public class CommentsViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string Author { get; set; }

        public DateTime DateCreated { get; set; }

        public int BugId { get; set; }

        public string BugTitle { get; set; }

        public static Expression<Func<Comment, CommentsViewModel>> Create
        {
            get
            {
                return c => new CommentsViewModel
                {
                    Id = c.Id,
                    Text = c.Text,
                    Author = c.CommentAuthor == null ? null : c.CommentAuthor.UserName,
                    DateCreated = c.CreatedOn,
                    BugId = c.BugId,
                    BugTitle = c.Bug.Title
                };
            }
        }
    }
}